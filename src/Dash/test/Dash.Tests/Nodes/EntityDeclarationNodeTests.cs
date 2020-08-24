// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Dash.Nodes;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Nodes
{
    public class EntityDeclarationNodeTests
    {
        [Fact]
        public void InsertAttributeDeclaration_IndexZero_ShouldBeInsertedAtTop()
        {
            // Arrange
            var sut = new EntityDeclarationNode(new ModelNode(), "Order");
            sut.AddAttributeDeclaration("Name", "String");

            // Act
            sut.InsertAttributeDeclaration(0, "Id", "Int");

            // Assert
            sut.AttributeDeclarations.Should().SatisfyRespectively(
                first =>
                {
                    first.AttributeName.Should().Be("Id");
                    first.AttributeDataType.Should().Be("Int");
                },
                second =>
                {
                    second.AttributeName.Should().Be("Name");
                    second.AttributeDataType.Should().Be("String");
                }
            );
        }
    }
}
