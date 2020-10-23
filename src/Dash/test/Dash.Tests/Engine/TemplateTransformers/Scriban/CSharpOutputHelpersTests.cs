// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using Dash.Engine.DataTypes;
using Dash.Engine.Models;
using Dash.Engine.Parsers.Result;
using Dash.Engine.TemplateTransformers.Scriban;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Engine.TemplateTransformers.Scriban
{
    public class GetCSharpLiteralFormatterTests
    {
        [Theory]
        [MemberData(nameof(Data), 2)]
        public void GetCSharpLiteral_Null_ShouldReturnValueNull(object value, string expectedOutput)
        {
            // Act
            var result = CSharpOutputHelpers.GetCSharpLiteral(value);

            // Assert
            result.Should().Be(expectedOutput);
        }

        public static IEnumerable<object?[]> Data =>
            new List<object?[]>
            {
                new object?[] { null, "null" },
                new object?[] { true, "true" },
                new object?[] { false, "false" },
                new object?[] { 1234, "1234" },
                new object?[] { 12.34m, "12.34m"},
                new object?[] { "", "\"\"" },
                new object?[] { "\"", "\"\\\"\"" },
                new object?[] { "foo", "\"foo\"" },
            };

        [Theory]
        [InlineData(true, "")]
        [InlineData(false, "= null!;")]
        public void GetPropertyDefaultValueAssignment_ReferencedEntity_ShouldReturnEmptyString(bool isNullable, string expectedOutput)
        {
            // Arrange
            var referencedEntity = new ReferencedEntityModel("Foo", "Bar", isNullable);

            // Act
            var result = CSharpOutputHelpers.GetPropertyDefaultValueAssignment(referencedEntity);

            // Assert
            result.Should().Be(expectedOutput);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetPropertyDefaultValueAssignment_AttributeWithIntDataType_ShouldReturnEmptyString(bool isNullable)
        {
            // Arrange
            var attribute = new AttributeModel("foo", new DataTypeParserResult(new IntDataType()).WithIsNullable(isNullable), "int");

            // Act
            var result = CSharpOutputHelpers.GetPropertyDefaultValueAssignment(attribute);

            // Assert
            result.Should().BeEmpty();
        }

        [Theory]
        [InlineData(true, "")]
        [InlineData(false, "= null!;")]
        public void GetPropertyDefaultValueAssignment_AttributeWithStringDataType_ShouldReturnResult(bool isNullable, string expectedOutput)
        {
            // Arrange
            var attribute = new AttributeModel("foo", new DataTypeParserResult(new StringDataType()).WithIsNullable(isNullable), "string");

            // Act
            var result = CSharpOutputHelpers.GetPropertyDefaultValueAssignment(attribute);

            // Assert
            result.Should().Be(expectedOutput);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetPropertyDefaultValueAssignment_AttributeWithBooleanDataType_ShouldReturnEmptyString(bool isNullable)
        {
            // Arrange
            var dataTypeParserResult = new DataTypeParserResult(new BoolDataType())
                .WithIsNullable(isNullable);

            var attribute = new AttributeModel("foo", dataTypeParserResult, "bool");

            // Act
            var result = CSharpOutputHelpers.GetPropertyDefaultValueAssignment(attribute);

            // Assert
            result.Should().BeEmpty();
        }
    }
}
