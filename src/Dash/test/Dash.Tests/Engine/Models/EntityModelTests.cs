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
            var superEntity = new EntityModel("Base")
                .WithAttribute<GuidDataType>("Id", "Guid")
                .WithAttribute<DateTimeDataType>("Created", "DateTime")
                .WithAttribute<StringDataType>("Name", "String");

            var sut = new EntityModel("Person")
                .WithAttribute<UnicodeDataType>("Name", "Unicode")
                .WithAttribute<StringDataType>("GivenName", "String")
                .WithAttribute<IntDataType>("Id", "Int");

            // Act
            sut.InheritAttributes(superEntity);

            // Assert
            sut.CodeAttributes.Should().SatisfyRespectively
            (
                first =>
                {
                    first.Name.Should().Be("Id");
                    first.TargetEnvironmentDataType.Should().Be("Int");
                },
                second =>
                {
                    second.Name.Should().Be("Created");
                    second.TargetEnvironmentDataType.Should().Be("DateTime");
                },
                third =>
                {
                    third.Name.Should().Be("Name");
                    third.TargetEnvironmentDataType.Should().Be("Unicode");
                },
                fourth =>
                {
                    fourth.Name.Should().Be("GivenName");
                    fourth.TargetEnvironmentDataType.Should().Be("String");
                }
            );
        }
    }
}
