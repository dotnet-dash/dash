// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Dash.Application;
using Dash.Common;
using Dash.PreprocessingSteps.Default;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace Dash.Tests.PreprocessingSteps
{
    public class FindProjectFileTests
    {
        private readonly MockFileSystem _mockFileSystem;
        private readonly IConsole _console;
        private readonly OptionsWrapper<DashOptions> _options;
        private readonly FindProjectFile _sut;

        public FindProjectFileTests()
        {
            _mockFileSystem = new MockFileSystem(null, "c:/temp");
            _mockFileSystem.AddFile("c:/temp/project.csproj", new MockFileData(string.Empty));
            _options = new OptionsWrapper<DashOptions>(new DashOptions
            {
                Project = null,
            });
            _console = Substitute.For<IConsole>();
            _sut = new FindProjectFile(_options, _console, _mockFileSystem);
        }

        [Fact]
        public async Task Process_NoProjectFoundInWorkingDirectory_ShouldOutputErrorToConsole()
        {
            // Arrange
            _mockFileSystem.Directory.SetCurrentDirectory("c:/");

            // Act
            await _sut.Process();

            // Assert
            _console.Received(1).Error("No .csproj file found in working directory.");
        }

        [Fact]
        public async Task Process_NoProjectFoundInWorkingDirectory_ShouldReturnFalse()
        {
            // Arrange
            _mockFileSystem.Directory.SetCurrentDirectory("c:/");

            // Act
            var result = await _sut.Process();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Process_ProjectFileFound_ShouldUpdateDashOptionsProjectFileProperty()
        {
            // Act
            _sut.Process();

            // Assert
            _options.Value.Project.Should().NotBeNull().And.Be(@"c:\temp\project.csproj");
        }

        [Fact]
        public void Process_MultipleProjectFilesFound_ShouldOutputErrorToConsole()
        {
            // Arrange
            _mockFileSystem.AddFile("c:/temp/project2.csproj", new MockFileData(string.Empty));

            // Act
            _sut.Process();

            // Assert
            _console.Received(1).Error("Multiple .csproj files found in working directory. Please specify the project explicitly.");
        }

        [Fact]
        public async Task Process_ProjectFileIsNotNull_ShouldReturnTrue()
        {
            // Arrange
            _options.Value.Project = "c:/foo.csproj";

            // Act
            var result = await _sut.Process();

            // Assert
            result.Should().BeTrue();
        }
    }
}