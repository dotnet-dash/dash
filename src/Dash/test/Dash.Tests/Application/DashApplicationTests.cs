using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Dash.Application;
using Dash.Engine.Abstractions;
using Dash.Engine.Models.SourceCode;
using Dash.Exceptions;
using Dash.Nodes;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Dash.Tests.Application
{
    public class DashApplicationTests
    {
        [Fact]
        public async Task Run_NoErrors_ShouldHaveCalledGenerate()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddFile("c:\\test.json", new MockFileData("{}"));

            var modelNode = new ModelNode();

            var sourceCodeDocument = new SourceCodeDocument(new Configuration(), modelNode);

            var sourceCodeParser = Substitute.For<ISourceCodeParser>();
            sourceCodeParser.Parse("{}").Returns(sourceCodeDocument);

            var nodeVisitors = new List<INodeVisitor> { Substitute.For<INodeVisitor>(), Substitute.For<INodeVisitor>(), Substitute.For<INodeVisitor>() };

            var generator = Substitute.For<IGenerator>();

            var sut = new DashApplication(
                mockFileSystem,
                sourceCodeParser,
                nodeVisitors,
                Substitute.For<IErrorRepository>(),
                generator,
                Substitute.For<IConsole>());

            // Act
            await sut.Run(new FileInfo("c:\\test.json"));

            // Assert
            await nodeVisitors[0].Received(1).Visit(modelNode);
            await nodeVisitors[1].Received(1).Visit(modelNode);
            await nodeVisitors[2].Received(1).Visit(modelNode);
            await generator.Received(1).Generate(sourceCodeDocument);
        }

        [Fact]
        public async Task Run_FileDoesNotExist_ShouldOutputError()
        {
            // Arrange
            var sourceCodeParser = Substitute.For<ISourceCodeParser>();
            sourceCodeParser.Parse("{}").Throws(new ParserException("Oops"));
            var console = Substitute.For<IConsole>();
            var sut = new DashApplication(new MockFileSystem(), sourceCodeParser, default, default, default, console);

            // Act
            await sut.Run(new FileInfo("c:\\file.json"));

            // Assert
            console.Received(1).Error("Could not find the model file 'c:\\file.json'.");
        }

        [Fact]
        public async Task Run_ParseExceptionThrown_ShouldOutputException()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile("c:\\file.json", new MockFileData("{}"));
            var sourceCodeParser = Substitute.For<ISourceCodeParser>();
            sourceCodeParser.Parse("{}").Throws(new ParserException("Oops"));
            var console = Substitute.For<IConsole>();
            var sut = new DashApplication(fileSystem, sourceCodeParser, default, default, default, console);

            // Act
            await sut.Run(new FileInfo("c:\\file.json"));

            // Assert
            console.Received(1).Error("Error while parsing the source code: Oops");
        }

        [Fact]
        public async Task Run_Error_ShouldStopAfterFirstVisitor()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddFile("c:\\test.json", new MockFileData("{}"));

            var sourceCodeParser = Substitute.For<ISourceCodeParser>();
            sourceCodeParser.Parse("{}").Returns(new SourceCodeDocument(new Configuration(), new ModelNode()));

            var visitors = new List<INodeVisitor>()
            {
                Substitute.For<INodeVisitor>(),
                Substitute.For<INodeVisitor>(),
            };

            var errorRepository = Substitute.For<IErrorRepository>();
            errorRepository.HasErrors().Returns(true);
            errorRepository.GetErrors().Returns(new List<string>
            {
                "An error"
            });

            var generator = Substitute.For<IGenerator>();

            var sut = new DashApplication(
                mockFileSystem,
                sourceCodeParser,
                visitors,
                errorRepository,
                generator,
                Substitute.For<IConsole>());

            // Act
            await sut.Run(new FileInfo("c:\\test.json"));

            // Assert
            await visitors[0].Received(1).Visit(Arg.Any<ModelNode>());
            await visitors[1].DidNotReceive().Visit(Arg.Any<ModelNode>());
            await generator.DidNotReceive().Generate(Arg.Any<SourceCodeDocument>());
        }
    }
}