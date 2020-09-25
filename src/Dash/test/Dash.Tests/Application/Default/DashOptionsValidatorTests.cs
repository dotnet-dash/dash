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

namespace Dash.Tests.Application.Default
{
    public class DashOptionsValidatorTests
    {
        private readonly IConsole _console = Substitute.For<IConsole>();
        private readonly MockFileSystem _mockFileSystem = new MockFileSystem();

        [Fact]
        public async Task Process_InputFileIsNull_ShouldWriteErrorToConsole()
        {
            // Arrange
            var sut = ArrangeSut(new DashOptions
            {
                InputFile = null
            });

            // Act
            await sut.Process();

            // Assert
            _console.Received(1).Error("Please specify a model file.");
        }


        [Fact]
        public async Task Process_InputFileIsNull_ShouldReturnFalse()
        {
            // Arrange
            var sut = ArrangeSut(new DashOptions
            {
                InputFile = null
            });

            // Act
            var result = await sut.Process();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Process_InputFileDoesNotExist_ShouldWriteErrorToConsole()
        {
            // Arrange
            var sut = ArrangeSut(new DashOptions
            {
                InputFile = "c:/temp/model.json",
            });

            // Act
            await sut.Process();

            // Assert
            _console.Received(1).Error("Could not find the model file 'c:/temp/model.json'.");
        }

        [Fact]
        public async Task Process_InputFileDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var sut = ArrangeSut(new DashOptions
            {
                InputFile = "c:/temp/model.json",
            });

            // Act
            var result = await sut.Process();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Process_ProjectFileDoesNotExist_ShouldWriteErrorToConsole()
        {
            // Arrange
            _mockFileSystem.AddFile("c:/temp/model.json", new MockFileData("{}"));

            var sut = ArrangeSut(new DashOptions
            {
                InputFile = "c:/temp/model.json",
                Project = "c:/temp/project.csproj"
            });

            // Act
            await sut.Process();

            // Assert
            _console.Received(1).Error("Could not find the .csproj file 'c:/temp/project.csproj'.");
        }

        private DashOptionsValidator ArrangeSut(DashOptions dashOptions)
        {
            var options = new OptionsWrapper<DashOptions>(dashOptions);

            return new DashOptionsValidator(_console, _mockFileSystem, options);
        }
    }
}
