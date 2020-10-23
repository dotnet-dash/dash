// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Dash.Engine;
using Dash.Engine.Models;
using Dash.Engine.Parsers.Result;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Engine.Models
{
    public class AttributeModelTests
    {
        [Fact]
        public void Ctor_Parameters_ShouldAssignProperties()
        {
            // Arrange
            var mockDataType = Substitute.For<IDataType>();

            var dataTypeParserResult = new DataTypeDeclarationParserResult(mockDataType)
                .WithIsNullable(true)
                .WithRegularExpression("RegularFoo")
                .WithMaximumLength(88)
                .WithDefaultValue("DefaultFoo");

            // Act
            var sut = new AttributeModel("Foo", dataTypeParserResult, "FooBar");

            // Assert
            sut.Name.Should().Be("Foo");
            sut.DataType.Should().Be(mockDataType);
            sut.DefaultValue.Should().Be("DefaultFoo");
            sut.IsNullable.Should().BeTrue();
            sut.MaxLength.Should().Be(88);
            sut.TargetEnvironmentDataType.Should().Be("FooBar");
        }
    }
}
