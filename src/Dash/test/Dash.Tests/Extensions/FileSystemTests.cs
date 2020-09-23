// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.IO.Abstractions.TestingHelpers;
using Dash.Application;
using Dash.Extensions;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Extensions
{
    public class FileSystemTests
    {
        [Fact]
        public void GetAbsoluteWorkingDirectory_NoProjectSpecified_ShouldReturnAbsolutePathOfProject()
        {
            // Arrange
            var sut = new MockFileSystem(null, "c:/temp/foo/bar");

            var dashOptions = new DashOptions
            {
                Project = null
            };

            // Act
            var result = sut.GetAbsoluteWorkingDirectory(dashOptions);

            // Assert
            result.Should().Be("c:/temp/foo/bar");
        }

        [Fact]
        public void GetAbsoluteWorkingDirectory_AbsoluteProjectSpecified_ShouldReturnAbsolutePathOfProject()
        {
            // Arrange
            var sut = new MockFileSystem();

            var dashOptions = new DashOptions
            {
                Project = "c:/temp/foobar/foo.csproj"
            };

            // Act
            var result = sut.GetAbsoluteWorkingDirectory(dashOptions);

            // Assert
            result.Should().Be(@"c:\temp\foobar");
        }

        [Fact]
        public void GetAbsoluteWorkingDirectory_RelativeProjectSpecified_ShouldReturnAbsolutePathOfProject()
        {
            // Arrange
            var sut = new MockFileSystem(null, "c:/temp/foo/bar");

            var dashOptions = new DashOptions
            {
                Project = "foo.csproj"
            };

            // Act
            var result = sut.GetAbsoluteWorkingDirectory(dashOptions);

            // Assert
            result.Should().Be(@"c:/temp/foo/bar");
        }

        [Theory]
        [InlineData("./foo", null, @"c:\temp\foo")]
        [InlineData("./foo", "c:/temp/foo/bar.csproj", @"c:\temp\foo\foo")]
        [InlineData("../foo", null, @"c:\foo")]
        [InlineData("../foo", "c:/temp/foo/bar.csproj", @"c:\temp\foo")]
        public void AbsolutePath_RelativeUri_ShouldReturnAbsolutePath(string relativePath, string? project, string expectedAbsolutePath)
        {
            // Arrange
            var sut = new MockFileSystem(null, "c:/temp");

            var uri = new Uri(relativePath, UriKind.Relative);

            // Act
            var result = sut.AbsolutePath(uri, new DashOptions { Project = project });

            // Assert
            result.Should().Be(expectedAbsolutePath);
        }

        [Fact]
        public void AbsolutePath_AbsoluteUri_ShouldReturnAbsolutePath()
        {
            // Arrange
            var sut = new MockFileSystem();

            var uri = new Uri("file:///c:/foo/bar");

            // Act
            var result = sut.AbsolutePath(uri, new DashOptions { Project = null });

            // Assert
            result.Should().Be(@"c:\foo\bar");
        }
    }
}
