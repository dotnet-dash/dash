// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using Dash.Nodes;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Engine.Nodes
{
    public class UriNodeTests
    {
        [Fact]
        public void ForExistingFile_SupportedSchemesShouldContainFileSchemeOnly()
        {
            // Act
            var result = UriNode.ForExistingFile(new Uri("file://c:/foo.csv"));

            // Assert
            result.SupportedSchemes.Should().BeEquivalentTo("file");
        }

        [Fact]
        public void ForExistingFile_UriMustExistShouldBeTrue()
        {
            // Act
            var result = UriNode.ForExistingFile(new Uri("file://c:/foo.csv"));

            // Assert
            result.UriMustExist.Should().BeTrue();
        }

        [Fact]
        public void ForOutputFile_SupportedSchemesShouldContainFileSchemeOnly()
        {
            // Act
            var result = UriNode.ForOutputFile(new Uri("file://c:/foo.csv"));

            // Assert
            result.SupportedSchemes.Should().BeEquivalentTo("file");
        }

        [Fact]
        public void ForOutputFile_UriMustExistShouldBeFalse()
        {
            // Act
            var result = UriNode.ForOutputFile(new Uri("file://c:/foo.csv"));

            // Assert
            result.UriMustExist.Should().BeFalse();
        }

        [Fact]
        public void ForExternalResources_SupportedSchemesShouldBeFileAndHttp()
        {
            // Act
            var result = UriNode.ForExternalResources(new Uri("https://foo"));

            // Assert
            result.SupportedSchemes.Should().SatisfyRespectively(
                first => first.Should().Be("file"),
                second => second.Should().Be("http"),
                third => third.Should().Be("https"));
        }

        [Fact]
        public void ForExternalResources_UriMustExistShouldBeTrue()
        {
            // Act
            var result = UriNode.ForExternalResources(new Uri("https://foo"));

            // Assert
            result.UriMustExist.Should().BeTrue();
        }

        [Fact]
        public void ForForInternalExternalResources_SupportedSchemesShouldBeFileHttpAndDash()
        {
            // Act
            var result = UriNode.ForInternalExternalResources(new Uri("dash://foo"));

            // Assert
            result.SupportedSchemes.Should().SatisfyRespectively(
                first => first.Should().Be("file"),
                second => second.Should().Be("http"),
                third => third.Should().Be("https"),
                fourth => fourth.Should().Be("dash"));
        }

        [Fact]
        public void ForForInternalExternalResources_UriMustExistShouldBeTrue()
        {
            // Act
            var result = UriNode.ForInternalExternalResources(new Uri("dash://foo"));

            // Assert
            result.UriMustExist.Should().BeTrue();
        }
    }
}
