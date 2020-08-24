// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Dash.Engine.Models;
using FluentAssertions;
using JetBrains.Annotations;
using Xunit;

namespace Dash.Tests.Engine.Models
{
    public class AttributeModelTests
    {
        [Theory]
        [InlineData("Id", "Int", false, "11")]
        [InlineData("Nickname", "String", true, null)]
        public void Ctor_Parameters_Should(string name, string dataType, bool isNullable, [CanBeNull] string defaultValue)
        {
            // Act
            var sut = new AttributeModel(name, dataType, isNullable, defaultValue);

            // Assert
            sut.Name.Should().Be(name);
            sut.DataType.Should().Be(dataType);
            sut.IsNullable.Should().Be(isNullable);
            sut.DefaultValue.Should().Be(defaultValue);
        }
    }
}
