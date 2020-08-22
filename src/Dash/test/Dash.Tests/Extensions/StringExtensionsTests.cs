using Dash.Extensions;
using FluentAssertions;
using System;
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
        [InlineData("https://www.foo.com")]
        [InlineData("file://localhost")]
        public void ToUri_ValidUri_ShouldReturnUri(string validUri)
        {
            // Act
            var result = validUri.ToUri();

            // Assert
            result.Should().Be(new Uri(validUri));
        }
    }
}
