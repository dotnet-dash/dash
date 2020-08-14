using Dash.Engine.Abstractions;
using Dash.Engine.JsonParser;
using Dash.Engine.LanguageProviders;
using FluentAssertions;
using Xunit;

namespace Dash.Tests
{
    public class DataTypeParserTests
    {
        private readonly DataTypeParser _sut;

        public DataTypeParserTests()
        {
            _sut = new DataTypeParser(new ILanguageProvider[]
            {
                new CSharpLanguageProvider(),
                new SqlServerLanguageProvider(),
            });
        }

        [Fact]
        public void Parse_ConstraintsNotSpecified_AllConstraintsShouldBeDefault()
        {
            // Act
            var attribute = _sut.Parse("Name", "unicode");

            // Assert
            attribute.IsNullable.Should().BeFalse();
            attribute.Length.Should().BeNull();
            attribute.DataTypeRegularExpression.Should().BeNull();
            attribute.RangeLowerBound.Should().BeNull();
            attribute.RangeUpperBound.Should().BeNull();
        }

        [Theory]
        [InlineData("unicode?")]
        [InlineData("unicode[100]?")]
        public void Parse_NullableIsSpecified_IsNullableShouldBeTrue(string dataTypeSpecification)
        {
            // Act
            var attribute = _sut.Parse("Name", dataTypeSpecification);

            // Assert
            attribute.Name.Should().Be("Name");
            attribute.CodeDataType.Should().Be("string");
            attribute.DatabaseDataType.Should().Be("nvarchar");
            attribute.IsNullable.Should().BeTrue();
        }

        [Theory]
        [InlineData("unicode[50]", 50)]
        [InlineData("unicode?[255]", 255)]
        [InlineData("unicode[150]?", 150)]
        public void Parse_LengthSpecified_LengthShouldBeSet(string dataTypeSpecification, int expectedLength)
        {
            // Act
            var attribute = _sut.Parse("Name", dataTypeSpecification);

            // Assert
            attribute.Length.Should().Be(expectedLength);
        }

        [Fact]
        public void Parse_DefaultValueSpecified_DefaultValueShouldBeSet()
        {
            // Act
            var attribute = _sut.Parse("Name", "unicode(==John Doe)");

            // Assert
            attribute.DefaultValue.Should().Be("John Doe");
        }

        [Theory]
        [InlineData("int[1..100]", 1, 100)]
        [InlineData("int[..100]", null, 100)]
        [InlineData("int[10..]", 10, null)]
        public void Parse_RangeSpecified_RangeShouldBeSet(string dataTypeSpecification, int? expectedLowerBound, int? expectedUpperBound)
        {
            // Act
            var attribute = _sut.Parse("Age", dataTypeSpecification);

            // Assert
            attribute.RangeLowerBound.Should().Be(expectedLowerBound);
            attribute.RangeUpperBound.Should().Be(expectedUpperBound);
        }

        [Theory]
        [InlineData("string:[a-z{5,10}", "[a-z{5,10}")]
        public void Parse_RegularExpressionSpecified_RegularExpressionShouldBeSet(string dataTypeSpecification, string expectedRegularExpression)
        {
            // Act
            var attribute = _sut.Parse("Username", dataTypeSpecification);

            // Assert
            attribute.DataTypeRegularExpression.Should().Be(expectedRegularExpression);
        }

        [Fact]
        public void Parse_ComplexSpecification_ShouldParseCorrectly()
        {
            // Act
            var attribute = _sut.Parse("Username", "string[200]?(==unknown):[a-zA-Z0-9]");

            // Assert
            attribute.Name.Should().Be("Username");
            attribute.CodeDataType.Should().Be("string");
            attribute.Length.Should().Be(200);
            attribute.IsNullable.Should().BeTrue();
            attribute.DefaultValue.Should().Be("unknown");
            attribute.DataTypeRegularExpression.Should().Be("[a-zA-Z0-9]");
            attribute.RangeLowerBound.Should().BeNull();
            attribute.RangeUpperBound.Should().BeNull();
        }
    }
}
