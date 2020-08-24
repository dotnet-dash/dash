// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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
    }
}
