// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Dash.Engine.DataTypes;
using Dash.Engine.Models;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Engine.Models
{
    public class EntityTests
    {
        [Fact]
        public void InheritAttributes()
        {
            // Arrange
            var superEntity = new EntityModel("Base");
            superEntity.CodeAttributes.Add(new AttributeModel("Id", new GuidDataType(), "Guid", false, null));
            superEntity.CodeAttributes.Add(new AttributeModel("Created", new DateTimeDataType(), "DateTime", false, null));
            superEntity.CodeAttributes.Add(new AttributeModel("Name", new StringDataType(), "String", false, null));

            var sut = new EntityModel("Person");
            sut.CodeAttributes.Add(new AttributeModel("Name", new UnicodeDataType(), "Unicode", false, null));
            sut.CodeAttributes.Add(new AttributeModel("GivenName", new StringDataType(), "String", false, null));
            sut.CodeAttributes.Add(new AttributeModel("Id", new IntDataType(), "Int", false, null));

            // Act
            sut.InheritAttributes(superEntity);

            // Assert
            sut.CodeAttributes.Should().SatisfyRespectively
            (
                first =>
                {
                    first.Name.Should().Be("Id");
                    first.DataType.Should().Be("Int");
                },
                second =>
                {
                    second.Name.Should().Be("Created");
                    second.DataType.Should().Be("DateTime");
                },
                third =>
                {
                    third.Name.Should().Be("Name");
                    third.DataType.Should().Be("Unicode");
                },
                fourth =>
                {
                    fourth.Name.Should().Be("GivenName");
                    fourth.DataType.Should().Be("String");
                }
            );
        }
    }
}
