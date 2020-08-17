using Dash.Engine.LanguageProviders;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Engine.LanguageProviders
{
    public class SqlServerLanguageProviderTests
    {
        [Theory]
        [InlineData("Int", "int")]
        [InlineData("Bool", "bit")]
        [InlineData("DateTime", "datetime")]
        [InlineData("Email", "nvarchar")]
        [InlineData("Guid", "uniqueidentifier")]
        [InlineData("String", "varchar")]
        [InlineData("Unicode", "nvarchar")]
        public void Translate_GivenDashDataType_ShouldReturnExpectedDataType(string dashDataType, string expectedDataType)
        {
            // Arrange
            var sut = new SqlServerLanguageProvider();

            // Act
            var result = sut.Translate(dashDataType);

            // Assert
            result.Should().Be(expectedDataType);
        }
    }
}
