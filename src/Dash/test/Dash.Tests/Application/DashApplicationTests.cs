using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Dash.Application;
using Dash.Engine.Abstractions;
using Dash.Engine.Models.SourceCode;
using Dash.Nodes;
using NSubstitute;
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
            mockFileSystem.AddFile("/test.json", new MockFileData("{}"));

            var modelNode = new ModelNode();

            var sourceCodeDocument = new SourceCodeDocument(new Configuration(), modelNode);

            var sourceCodeParser = Substitute.For<ISourceCodeParser>();
            sourceCodeParser.Parse("{}").Returns(sourceCodeDocument);

            var nodeVisitors = new List<INodeVisitor>
            {
                Substitute.For<INodeVisitor>(),
                Substitute.For<INodeVisitor>(),
                Substitute.For<INodeVisitor>(),
            };

            var generator = Substitute.For<IGenerator>();

            var sut = new DashApplication(
                mockFileSystem,
                sourceCodeParser,
                nodeVisitors,
                Substitute.For<IErrorRepository>(),
                generator,
                Substitute.For<IConsole>());

            // Act
            await sut.Run(new FileInfo("/test.json"), false);

            // Assert
            nodeVisitors[0].Received(1).Visit(modelNode);
            nodeVisitors[1].Received(1).Visit(modelNode);
            nodeVisitors[2].Received(1).Visit(modelNode);
            await generator.Received(1).Generate(sourceCodeDocument);
        }

        [Fact]
        public async Task Run_X_Y()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddFile("/test.json", new MockFileData("{}"));

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
            await sut.Run(new FileInfo("/test.json"), false);

            // Assert
            visitors[0].Received(1).Visit(Arg.Any<ModelNode>());
            visitors[1].DidNotReceive().Visit(Arg.Any<ModelNode>());
            await generator.DidNotReceive().Generate(Arg.Any<SourceCodeDocument>());
        }
    }
}
