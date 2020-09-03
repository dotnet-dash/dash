// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Dash.Application;
using Dash.Common;
using Dash.Engine;
using Dash.Exceptions;
using Dash.Nodes;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Dash.Tests.Application
{
    public class DashApplicationTests
    {
        private readonly MockFileSystem _mockFileSystem = new MockFileSystem();
        private readonly DashApplication _sut;
        private readonly ISourceCodeParser _sourceCodeParser = Substitute.For<ISourceCodeParser>();
        private readonly IConsole _console = Substitute.For<IConsole>();
        private readonly IList<INodeVisitor> _nodeVisitors = new List<INodeVisitor>();
        private readonly IGenerator _generator = Substitute.For<IGenerator>();
        private readonly IList<IPostGenerator> _postGenerators = new List<IPostGenerator>();
        private readonly IErrorRepository _errorRepository = Substitute.For<IErrorRepository>();

        public DashApplicationTests()
        {
            _sut = new DashApplication(
                _mockFileSystem,
                _sourceCodeParser,
                _nodeVisitors,
                _errorRepository,
                _generator,
                _postGenerators,
                _console);
        }

        [Fact]
        public async Task Run_NoErrors_ShouldHaveCalledGenerate()
        {
            // Arrange
            _mockFileSystem.AddFile("c:\\test.json", new MockFileData("{}"));

            var sourceCodeNode = new SourceCodeNode(new ConfigurationNode(), new ModelNode());
            _sourceCodeParser.Parse("{}").Returns(sourceCodeNode);

            _nodeVisitors.Add(Substitute.For<INodeVisitor>());
            _nodeVisitors.Add(Substitute.For<INodeVisitor>());
            _nodeVisitors.Add(Substitute.For<INodeVisitor>());

            // Act
            await _sut.Run("c:\\test.json");

            // Assert
            await _nodeVisitors[0].Received(1).Visit(sourceCodeNode);
            await _nodeVisitors[1].Received(1).Visit(sourceCodeNode);
            await _nodeVisitors[2].Received(1).Visit(sourceCodeNode);
            await _generator.Received(1).Generate(sourceCodeNode);
        }

        [Fact]
        public async Task Run_FileDoesNotExist_ShouldOutputError()
        {
            // Arrange
            var sourceCodeParser = Substitute.For<ISourceCodeParser>();
            sourceCodeParser.Parse("{}").Throws(new ParserException("Oops"));

            // Act
            await _sut.Run("c:\\file.json");

            // Assert
            _console.Received(1).Error("Could not find the model file 'c:\\file.json'.");
        }

        [Fact]
        public async Task Run_ParseExceptionThrown_ShouldOutputException()
        {
            // Arrange
            _mockFileSystem.AddFile("c:\\file.json", new MockFileData("{}"));
            _sourceCodeParser.Parse("{}").Throws(new ParserException("Oops"));

            // Act
            await _sut.Run("c:\\file.json");

            // Assert
            _console.Received(1).Error("Error while parsing the source code: Oops");
        }

        [Fact]
        public async Task Run_Error_ShouldStopAfterFirstVisitor()
        {
            // Arrange
            _mockFileSystem.AddFile("c:\\test.json", new MockFileData("{}"));

            _sourceCodeParser.Parse("{}").Returns(new SourceCodeNode(new ConfigurationNode(), new ModelNode()));

            _nodeVisitors.Add(Substitute.For<INodeVisitor>());
            _nodeVisitors.Add(Substitute.For<INodeVisitor>());

            _errorRepository.HasErrors().Returns(true);
            _errorRepository.GetErrors().Returns(new List<string>
            {
                "An error"
            });

            var generator = Substitute.For<IGenerator>();

            var sut = new DashApplication(
                _mockFileSystem,
                _sourceCodeParser,
                _nodeVisitors,
                _errorRepository,
                generator,
                _postGenerators,
                Substitute.For<IConsole>());

            // Act
            await sut.Run("c:\\test.json");

            // Assert
            await _nodeVisitors[0].Received(1).Visit(Arg.Any<SourceCodeNode>());
            await _nodeVisitors[1].DidNotReceive().Visit(Arg.Any<SourceCodeNode>());
            await generator.DidNotReceive().Generate(Arg.Any<SourceCodeNode>());
        }
    }
}