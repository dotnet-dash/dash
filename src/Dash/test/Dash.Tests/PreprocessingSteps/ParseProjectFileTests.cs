// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using Dash.Application;
using Dash.PreprocessingSteps.Default;
using Dash.Roslyn;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Dash.Tests.PreprocessingSteps
{
    public class ParseProjectFileTests
    {
        [Fact]
        public async Task Process_OpenProjectAsyncReturnsNull_ShouldReturnFalse()
        {
            // Arrange
            var workspace = Substitute.For<IWorkspace>();
            workspace.OpenProjectAsync().ReturnsNull();

            var sut = new ParseProjectFile(Substitute.For<IOptions<DashOptions>>(), workspace);

            // Act
            var result = await sut.Process();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Process_OpenProjectAsyncReturnsObject_ShouldReturnTrue()
        {
            // Arrange
            var workspace = Substitute.For<IWorkspace>();
            workspace.OpenProjectAsync().Returns(Substitute.For<IProject>());

            var sut = new ParseProjectFile(new OptionsWrapper<DashOptions>(new DashOptions()), workspace);

            // Act
            var result = await sut.Process();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Process_OpenProjectAsyncReturnsObject_ShouldSetOptionsDefaultNamespace()
        {
            // Arrange
            var options = new OptionsWrapper<DashOptions>(new DashOptions());

            var project = Substitute.For<IProject>();
            project.Namespace.Returns("Foo");

            var workspace = Substitute.For<IWorkspace>();
            workspace.OpenProjectAsync().Returns(project);

            var sut = new ParseProjectFile(options, workspace);

            // Act
            await sut.Process();

            // Assert
            options.Value.DefaultNamespace.Should().Be("Foo");
        }
    }
}