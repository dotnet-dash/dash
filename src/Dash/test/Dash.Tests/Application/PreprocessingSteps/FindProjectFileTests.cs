// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Dash.Application;
using Dash.Application.PreprocessingSteps;
using Dash.Common;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Application.PreprocessingSteps
{
    public class FindProjectFileTests
    {
        private readonly MockFileSystem _mockFileSystem;
        private readonly IConsole _console;

        public FindProjectFileTests()
        {
            _mockFileSystem = new MockFileSystem(null, "c:/temp");
            _mockFileSystem.AddFile("c:/temp/project.csproj", new MockFileData(string.Empty));

            _console = Substitute.For<IConsole>();
        }

        [Fact]
        public async Task Process_NoProjectFileFound_ShouldOutputErrorToConsole()
        {
            // Arrange
            var options = new OptionsWrapper<DashOptions>(new DashOptions
            {
                ProjectFile = null,
                WorkingDirectory = "c:/",
            });

            var sut = new FindProjectFile(options, _console, _mockFileSystem);

            // Act
            await sut.Process();

            // Assert
            _console.Received(1).Error("No .csproj file found in working directory.");
        }

        [Fact]
        public async Task Process_NoProjectFileFound_ShouldReturnFalse()
        {
            // Arrange
            _mockFileSystem.AddDirectory("c:/temp/xxx");

            var options = new OptionsWrapper<DashOptions>(new DashOptions
            {
                ProjectFile = null,
                WorkingDirectory = "c:/temp/xxx",
            });

            var sut = new FindProjectFile(options, _console, _mockFileSystem);

            // Act
            var result = await sut.Process();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Process_ProjectFileFound_ShouldUpdateDashOptionsProjectFileProperty()
        {
            // Arrange
            var dashOptions = new DashOptions
            {
                ProjectFile = null,
            };

            var sut = new FindProjectFile(new OptionsWrapper<DashOptions>(dashOptions), Substitute.For<IConsole>(), _mockFileSystem);

            // Act
            sut.Process();

            // Assert
            dashOptions.ProjectFile.Should().NotBeNull().And.Be(@"c:\temp\project.csproj");
        }

        [Fact]
        public void Process_MultipleProjectFilesFound_ShouldOutputErrorToConsole()
        {
            // Arrange
            _mockFileSystem.AddFile("c:/temp/project2.csproj", new MockFileData(string.Empty));

            var options = new OptionsWrapper<DashOptions>(new DashOptions
            {
                ProjectFile = null,
            });

            var sut = new FindProjectFile(options, _console, _mockFileSystem);

            // Act
            sut.Process();

            // Assert
            _console.Received(1).Error("Multiple .csproj files found in working directory.");
        }

        [Fact]
        public async Task Process_ProjectFileIsNull_ShouldReturnTrue()
        {
            // Arrange
            var options = new OptionsWrapper<DashOptions>(new DashOptions
            {
                ProjectFile = "c:/foo.csproj",
            });

            var sut = new FindProjectFile(options, _console, _mockFileSystem);

            // Act
            var result = await sut.Process();

            // Assert
            result.Should().BeTrue();
        }
    }
}
