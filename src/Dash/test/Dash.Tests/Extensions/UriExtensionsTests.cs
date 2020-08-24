﻿// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using Dash.Common;
using Dash.Extensions;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Extensions
{
    public class UriExtensionsTests
    {
        [Fact]
        public void ToPath_RelativeUri_ShouldConvert()
        {
            // Arrange
            var sessionService = Substitute.For<ISessionService>();
            sessionService.GetWorkingDirectory().Returns("c:/temp/");

            var uri = new Uri("../", UriKind.Relative);

            // Act
            var result = uri.ToPath(sessionService);

            // Assert
            result.Should().Be("c:/temp/../");
        }

        [Theory]
        [InlineData("file:///foo/bar", "/foo/bar")]
        [InlineData("file:///c:/foo/bar", "c:/foo/bar")]
        public void ToAbsolutePath_AbsoluteUri_Should(string absoluteUri, string expectedPath)
        {
            // Arrange
            var uri = new Uri(absoluteUri, UriKind.Absolute);

            // Act
            var result = uri.ToPath(Substitute.For<ISessionService>());

            // Assert
            result.Should().Be(expectedPath);
        }
    }
}