using Dash.Engine.JsonParser;
using FluentAssertions;
using Xunit;

namespace Dash.Tests
{
    public class DataTypeParserTests
    {
        [Fact]
        public void Parse_ConstraintsNotSpecified_AllConstraintsShouldBeDefault()
        {
            // Arrange
            var sut = new DataTypeParser();

            // Act
            var x = sut.Parse("Name", "unicode");

            // Assert
            x.IsNullable.Should().BeFalse();
            x.Length.Should().BeNull();
            x.DataTypeRegularExpression.Should().BeNull();
            x.RangeLowerBound.Should().BeNull();
            x.RangeUpperBound.Should().BeNull();
        }

        [Theory]
        [InlineData("unicode?")]
        [InlineData("unicode[100]?")]
        public void Parse_NullableIsSpecified_IsNullableShouldBeTrue(string dataTypeSpecification)
        {
            // Arrange
            var sut = new DataTypeParser();

            // Act
            var x = sut.Parse("Name", dataTypeSpecification);

            // Assert
            x.Name.Should().Be("Name");
            x.CodeDataType.Should().Be("unicode");
            x.IsNullable.Should().BeTrue();
        }

        [Theory]
        [InlineData("unicode[50]", 50)]
        [InlineData("unicode?[255]", 255)]
        [InlineData("unicode[150]?", 150)]
        public void Parse_LengthSpecified_LengthShouldBeSet(string dataTypeSpecification, int expectedLength)
        {
            // Arrange
            var sut = new DataTypeParser();

            // Act
            var x = sut.Parse("Name", dataTypeSpecification);

            // Assert
            x.Length.Should().Be(expectedLength);
        }

        [Fact]
        public void Parse_DefaultValueSpecified_DefaultValueShouldBeSet()
        {
            // Arrange
            var sut = new DataTypeParser();

            // Act
            var result = sut.Parse("Name", "unicode(==John Doe)");

            // Assert
            result.DefaultValue.Should().Be("John Doe");
        }

        [Theory]
        [InlineData("int[1..100]", 1, 100)]
        [InlineData("int[..100]", null, 100)]
        [InlineData("int[10..]", 10, null)]
        public void Parse_RangeSpecified_RangeShouldBeSet(string dataTypeSpecification, int? expectedLowerBound, int? expectedUpperBound)
        {
            // Arrange
            var sut = new DataTypeParser();

            // Act
            var result = sut.Parse("Age", dataTypeSpecification);

            // Assert
            result.RangeLowerBound.Should().Be(expectedLowerBound);
            result.RangeUpperBound.Should().Be(expectedUpperBound);
        }

        [Theory]
        [InlineData("string:[a-z{5,10}", "[a-z{5,10}")]
        public void Parse_RegularExpressionSpecified_RegularExpressionShouldBeSet(string dataTypeSpecification, string expectedRegularExpression)
        {
            // Arrange
            var sut = new DataTypeParser();

            // Act
            var result = sut.Parse("Username", dataTypeSpecification);

            // Assert
            result.DataTypeRegularExpression.Should().Be(expectedRegularExpression);
        }

        [Fact]
        public void Parse_ComplexSpecification_ShouldParseCorrectly()
        {
            // Arrange
            var sut = new DataTypeParser();

            // Act
            var result = sut.Parse("Username", "string[200]?(==unknown):[a-zA-Z0-9]");

            // Assert
            result.Name.Should().Be("Username");
            result.CodeDataType.Should().Be("string");
            result.Length.Should().Be(200);
            result.IsNullable.Should().BeTrue();
            result.DefaultValue.Should().Be("unknown");
            result.DataTypeRegularExpression.Should().Be("[a-zA-Z0-9]");
            result.RangeLowerBound.Should().BeNull();
            result.RangeUpperBound.Should().BeNull();
        }
    }
}
