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
        public void GetAbsoluteWorkingDirectory_SingleDot_ShouldReturnCurrentDirectory()
        {
            // Arrange
            var sut = new MockFileSystem(null, "c:/temp/foo/");

            var dashOptions = new DashOptions
            {
                WorkingDirectory = "."
            };

            // Act
            var result = sut.GetAbsoluteWorkingDirectory(dashOptions);

            // Assert
            result.Should().Be("c:/temp/foo/");
        }

        [Fact]
        public void GetAbsoluteWorkingDirectory_TwoDots_ShouldReturnParentDirectory()
        {
            // Arrange
            var sut = new MockFileSystem(null, "c:/temp/foo/");

            var dashOptions = new DashOptions
            {
                WorkingDirectory = ".."
            };

            // Act
            var result = sut.GetAbsoluteWorkingDirectory(dashOptions);

            // Assert
            result.Should().Be(@"c:\temp");
        }

        [Fact]
        public void GetAbsoluteWorkingDirectory_AbsoluteDirectory_ShouldReturnAbsoluteDirectory()
        {
            // Arrange
            var sut = new MockFileSystem(null, @"c:\");

            var dashOptions = new DashOptions
            {
                WorkingDirectory = @"c:/temp/foobar/"
            };

            // Act
            var result = sut.GetAbsoluteWorkingDirectory(dashOptions);

            // Assert
            result.Should().Be(@"c:\temp\foobar");
        }

        [Fact]
        public void AbsolutePath_UriIsRelative_ShouldReturnAbsolutePath()
        {
            // Arrange
            var sut = new MockFileSystem();

            var uri = new Uri("file:///foo/bar").MakeRelativeUri(new Uri("file:///foo"));

            var options = new DashOptions
            {
                WorkingDirectory = "c:/temp/bar/"
            };

            // Act
            var result = sut.AbsolutePath(uri, options);

            // Assert
            result.Should().Be(@"c:\temp\foo");
        }

        [Fact]
        public void AbsolutePath_UriIsAbsolute_ShouldReturnAbsolutePath()
        {
            // Arrange
            var sut = new MockFileSystem();

            var uri = new Uri("file:///c:/foo/bar");

            // Act
            var result = sut.AbsolutePath(uri, new DashOptions());

            // Assert
            result.Should().Be(@"c:\foo\bar");
        }
    }
}
