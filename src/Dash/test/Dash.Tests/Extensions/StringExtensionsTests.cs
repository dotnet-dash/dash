// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using Dash.Extensions;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData(null, null, true)]
        [InlineData("a", "a", true)]
        [InlineData("A", "a ", false)]
        [InlineData("a", "b", false)]
        public void IsSame_GivenTheory_ShouldSatisfyExpectedResults(string sut, string otherString, bool expectedResult)
        {
            // Act
            var result = sut.IsSame(otherString);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public void IsValidUri_UriIsValid_ShouldReturnTrue()
        {
            // Act
            var result = "https://www.foo.bar".IsValidUri();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsValidUri_UriIsInvalid_ShouldReturnFalse()
        {
            // Act
            var result = "foobar".IsValidUri();

            // Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData("invalid")]
        public void ToUri_InvalidUri_ShouldReturnNull(string invalidUri)
        {
            // Act
            var result = invalidUri.ToUri();

            // Assert
            result.Should().BeNull();
        }

        [Theory]
        [InlineData("https://www.foo.com", "https://www.foo.com/")]
        [InlineData("file://localhost", "file://localhost/")]
        [InlineData("./", "./")]
        [InlineData("../", "../")]
        public void ToUri_ValidUri_ShouldReturnUri(string validUri, string expectedUri)
        {
            // Act
            var result = validUri.ToUri();

            // Assert
            result.Should().NotBeNull().And.Subject.ToString().Should().Be(expectedUri);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData("/foo/", "/foo/")]
        public void NormalizeSlashes_GivenTheory_ShouldReturnExpectedResults(string input, string expectedResult)
        {
            // Act
            var result = input.NormalizeSlashes();

            // Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public void NormalizeSlashes_EndsWithMultipleSlashes_ShouldReduceToOneSlash()
        {
            // Arrange
            var input = @"//foo//bar\\//\\//";

            // Act
            var result = input.NormalizeSlashes();

            // Assert
            result.Should().Be("//foo//bar/");
        }

        [Theory]
        [InlineData(@"c:\foo\bar.txt", @"c:/foo/bar.txt")]
        [InlineData(@"c:\foo\.\bar.txt", @"c:/foo/bar.txt")]
        [InlineData(@"c:\foo\.\.\bar.txt", @"c:/foo/bar.txt")]
        [InlineData(@"c:\foo\..\bar.txt", @"c:/bar.txt")]
        public void AbsolutePath_GivenInput_ShouldReturnExpected(string input, string expected)
        {
            // Act
            var result = input.AbsolutePath();

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void AbsolutePath_RelativePath_ShouldThrowInvalidOperationException()
        {
            // Act
            Action act = () => "./foo.txt".AbsolutePath();

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }
    }
}