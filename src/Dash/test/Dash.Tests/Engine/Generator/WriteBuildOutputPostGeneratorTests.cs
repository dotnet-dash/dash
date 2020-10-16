// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading.Tasks;
using Dash.Engine;
using Dash.Engine.Generator;
using FluentArrange.NSubstitute;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Engine.Generator
{
    public class WriteBuildOutputPostGeneratorTests
    {
        [Fact]
        public async Task Run_NoBuildOutputs_ShouldWriteNothing()
        {
            // Act
            var context = Arrange.For<WriteBuildOutputPostGenerator>()
                .WithDependency<IFileSystem>(new MockFileSystem());

            await context.Sut.Run();

            // Assert
            context.Dependency<IFileSystem, MockFileSystem>().AllFiles.Count().Should().Be(0);
        }

        [Fact]
        public async Task Run_BuildOutputToExistingPath_ShouldWriteToFileSystem()
        {
            // Assert
            var context = Arrange.For<WriteBuildOutputPostGenerator>()
                .WithDependency<IBuildOutputRepository>(repository =>
                {
                    repository.GetOutputItems().Returns(new List<BuildOutput>
                    {
                        new BuildOutput("c:/foo.cs", "Foo")
                    });
                })
                .WithDependency<IFileSystem>(new MockFileSystem());

            // Act
            await context.Sut.Run();

            // Assert
            context.Dependency<IFileSystem, MockFileSystem>().GetFile("c:/foo.cs").TextContents.Should().Be("Foo");
        }

        [Fact]
        public async Task Run_BuildOutputToNonExistingPath_ShouldWriteToFileSystem()
        {
            // Assert
            var context = Arrange.For<WriteBuildOutputPostGenerator>()
                .WithDependency<IBuildOutputRepository>(repository =>
                {
                    repository.GetOutputItems().Returns(new List<BuildOutput>
                    {
                        new BuildOutput("c:/foo/bar.cs", "Foobar")
                    });
                })
                .WithDependency<IFileSystem>(new MockFileSystem());

            // Act
            await context.Sut.Run();

            // Assert
            context.Dependency<IFileSystem, MockFileSystem>().GetFile("c:/foo/bar.cs").TextContents.Should().Be("Foobar");
        }
    }
}
