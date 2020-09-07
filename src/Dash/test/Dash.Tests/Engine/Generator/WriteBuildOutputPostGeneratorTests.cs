// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading.Tasks;
using Dash.Common;
using Dash.Engine;
using Dash.Engine.Generator;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Engine.Generator
{
    public class WriteBuildOutputPostGeneratorTests
    {
        private readonly IConsole _console = Substitute.For<IConsole>();
        private readonly IBuildOutputRepository _buildOutputRepository = Substitute.For<IBuildOutputRepository>();
        private readonly MockFileSystem _fileSystem = new MockFileSystem();
        private readonly WriteBuildOutputPostGenerator _sut;

        public WriteBuildOutputPostGeneratorTests()
        {
            _sut = new WriteBuildOutputPostGenerator(_console, _buildOutputRepository, _fileSystem);
        }

        [Fact]
        public async Task Run_NoBuildOutputs_ShouldWriteNothing()
        {
            // Act
            await _sut.Run();

            // Assert
            _fileSystem.AllFiles.Count().Should().Be(0);
        }

        [Fact]
        public async Task Run_BuildOutputToExistingPath_ShouldWriteToFileSystem()
        {
            // Assert
            var buildOutputs = new List<BuildOutput>
            {
                new BuildOutput("c:/foo.cs", "Foo")
            };

            _buildOutputRepository.GetOutputItems().Returns(buildOutputs);

            // Act
            await _sut.Run();

            // Assert
            _fileSystem.GetFile("c:/foo.cs").TextContents.Should().Be("Foo");
        }

        [Fact]
        public async Task Run_BuildOutputToNonExistingPath_ShouldWriteToFileSystem()
        {
            // Assert
            var buildOutputs = new List<BuildOutput>
            {
                new BuildOutput("c:/foo/bar.cs", "Foobar")
            };

            _buildOutputRepository.GetOutputItems().Returns(buildOutputs);

            // Act
            await _sut.Run();

            // Assert
            _fileSystem.GetFile("c:/foo/bar.cs").TextContents.Should().Be("Foobar");
        }
    }
}
