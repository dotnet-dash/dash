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
    }
}
