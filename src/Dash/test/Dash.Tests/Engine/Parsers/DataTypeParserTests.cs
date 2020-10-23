// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using Dash.Engine.Parsers;
using Dash.Exceptions;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Engine.Parsers
{
    public class DataTypeParserTests
    {
        private readonly DataTypeParser _sut;

        public DataTypeParserTests()
        {
            _sut = new DataTypeParser();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Parse_InvalidDataTypeSpecification_ShouldReturn(string invalidDataTypeSpecification)
        {
            // Arrange
            var sut = new DataTypeParser();

            // Act
            Action act = () => sut.Parse(invalidDataTypeSpecification);

            // Assert
            act.Should().Throw<InvalidDataTypeException>(invalidDataTypeSpecification);
        }

        [Fact]
        public void Parse_ConstraintsNotSpecified_AllConstraintsShouldBeDefault()
        {
            // Act
            var result = _sut.Parse("unicode");

            // Assert
            result.IsNullable.Should().BeFalse();
            result.Length.Should().BeNull();
            result.DataTypeRegularExpression.Should().BeNull();
            result.RangeLowerBound.Should().BeNull();
            result.RangeUpperBound.Should().BeNull();
        }

        [Theory]
        [InlineData("unicode?")]
        [InlineData("unicode[100]?")]
        public void Parse_NullableIsSpecified_IsNullableShouldBeTrue(string dataTypeSpecification)
        {
            // Act
            var result = _sut.Parse(dataTypeSpecification);

            // Assert
            result.IsNullable.Should().BeTrue();
        }

        [Theory]
        [InlineData("unicode[50]", 50)]
        [InlineData("unicode?[255]", 255)]
        [InlineData("unicode[150]?", 150)]
        public void Parse_LengthSpecified_LengthShouldBeSet(string dataTypeSpecification, int expectedLength)
        {
            // Act
            var result = _sut.Parse(dataTypeSpecification);

            // Assert
            result.Length.Should().Be(expectedLength);
        }

        [Theory]
        [InlineData("unicode(==' ')", " ")]
        [InlineData("unicode(=='John Doe')", "John Doe")]
        [InlineData("unicode(=='=')", "=")]
        [InlineData("unicode(=='(')", "(")]
        [InlineData(@"unicode(=='\')", "\\")]
        [InlineData(@"unicode(==''')", @"'")]
        [InlineData(@"unicode(=='''')", "''")]
        public void Parse_DefaultValueSpecified_DefaultValueShouldBeSet(string dataType, string expectedDefaultValue)
        {
            // Act
            var result = _sut.Parse(dataType);

            // Assert
            result.DefaultValue.Should().Be(expectedDefaultValue);
        }

        [Theory]
        [InlineData("int[1..100]", 1, 100)]
        [InlineData("int[..100]", null, 100)]
        [InlineData("int[10..]", 10, null)]
        public void Parse_RangeSpecified_RangeShouldBeSet(string dataTypeSpecification, int? expectedLowerBound, int? expectedUpperBound)
        {
            // Act
            var result = _sut.Parse(dataTypeSpecification);

            // Assert
            result.RangeLowerBound.Should().Be(expectedLowerBound);
            result.RangeUpperBound.Should().Be(expectedUpperBound);
        }

        [Theory]
        [InlineData("string:[a-z{5,10}", "[a-z{5,10}")]
        public void Parse_RegularExpressionSpecified_RegularExpressionShouldBeSet(string dataTypeSpecification, string expectedRegularExpression)
        {
            // Act
            var result = _sut.Parse(dataTypeSpecification);

            // Assert
            result.DataTypeRegularExpression.Should().Be(expectedRegularExpression);
        }

        [Fact]
        public void Parse_ComplexSpecification_ShouldParseCorrectly()
        {
            // Act
            var result = _sut.Parse("string[200]?(=='unknown'):[a-zA-Z0-9]");

            // Assert
            result.DataType.Name.Should().Be("string");
            result.Length.Should().Be(200);
            result.IsNullable.Should().BeTrue();
            result.DefaultValue.Should().Be("unknown");
            result.DataTypeRegularExpression.Should().Be("[a-zA-Z0-9]");
            result.RangeLowerBound.Should().BeNull();
            result.RangeUpperBound.Should().BeNull();
        }

        [Theory]
        [InlineData("string??")]
        [InlineData("int[1..10][2..10]")]
        public void Parse_ConstraintDefinedTwice_ShouldThrowInvalidDataTypeException(string dataTypeConstraintDefinedTwice)
        {
            // Act
            Action act = () => _sut.Parse(dataTypeConstraintDefinedTwice);

            // Assert
            act.Should().Throw<InvalidDataTypeConstraintException>()
                .And.Message.Should().Be("Constraints cannot be defined more than once");
        }
    }
}
