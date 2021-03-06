﻿// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Dash.Engine;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Engine
{
    public class BuildOutputTests
    {
        [Fact]
        public void Ctor_PathContent_ShouldAssignToProperties()
        {
            // Arrange
            var path = "c:/temp/foo.cs";
            var content = "public class Foobar {}";

            // Act
            var sut = new BuildOutput(path, content);

            // Assert
            sut.Path.Should().Be(@"c:/temp/foo.cs");
            sut.GeneratedSourceCodeContent.Should().Be(content);
        }

        [Fact]
        public void Equals_Object_IsNull_ShouldReturnFalse()
        {
            // Arrange
            var sut = new BuildOutput("c:/foo", "foo");

            // Act
            var result = sut.Equals((object?) null);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Equals_Object_IsItself_ShouldReturnTrue()
        {
            // Arrange
            var sut = new BuildOutput("c:/foo", "foo");

            // Act
            var result = sut.Equals((object) sut);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Equals_Object_IsDifferentType_ShouldReturnFalse()
        {
            // Arrange
            var sut = new BuildOutput("c:/foo", "foo");
            var s = (object) "c:/foo";

            // Act
            var result = sut.Equals(s);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Equals_Object_SamePath_ShouldReturnTrue()
        {
            // Arrange
            var sut = new BuildOutput("c:/foo", "foo");
            var buildOutput = new BuildOutput("c:/foo", "foo");

            // Act
            var result = sut.Equals((object) buildOutput);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Equals_Object_ItemHasDifferentPath_ShouldReturnFalse()
        {
            // Arrange
            var sut = new BuildOutput("c:/foo", "foo");
            var buildOutput = new BuildOutput("c:/bar", "foo");

            // Act
            var result = sut.Equals(buildOutput);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Equals_BuildOutput_HasSamePath_ShouldReturnTrue()
        {
            // Arrange
            var sut = new BuildOutput("c:/foo", "foo");
            var buildOutput = new BuildOutput("c:/foo", "foo");

            // Act
            var result = sut.Equals(buildOutput);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void GetHashCode_SamePath_ShouldProduceTheSameHashCode()
        {
            // Arrange
            var sut = new BuildOutput("c:/foo.bar", "foo");
            var hashCode = new BuildOutput("c:/foo.bar", "foo bar").GetHashCode();

            // Act
            var result = sut.GetHashCode();

            // Assert
            result.Should().Be(hashCode);
        }
    }
}
