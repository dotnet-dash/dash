// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using Dash.Engine;
using Dash.Engine.Visitors;
using Dash.Nodes;
using FluentArrange.NSubstitute;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Engine.Visitors
{
    public class DefaultSymbolCollectorTests
    {
        [Fact]
        public async Task Visit_EntityDeclarationNode_ShouldCallSymbolRepositoryAddEntity()
        {
            // Arrange
            var context = Arrange.For<DefaultSymbolCollector>();

            var modelNode = new ModelNode();

            // Act
            await context.Sut.Visit(new EntityDeclarationNode(modelNode, "Foo"));

            // Assert
            context.Dependency<ISymbolRepository>().Received(1).AddEntity("Foo");
        }

        [Fact]
        public async Task Visit_ModelNode_ShouldCallSymbolRepositoryAddEntityAttribute()
        {
            // Arrange
            var context = Arrange.For<DefaultSymbolCollector>();

            var modelNode = new ModelNode();
            modelNode
                .AddEntityDeclarationNode("Order")
                .AddAttributeDeclaration("Description", "String");

            modelNode
                .AddEntityDeclarationNode("OrderLine")
                .AddAttributeDeclaration("Quantity", "Int");

            // Act
            await context.Sut.Visit(modelNode);

            // Assert
            context.Dependency<ISymbolRepository>().Received(1).AddEntityAttribute("Order", "Description");
            context.Dependency<ISymbolRepository>().Received(1).AddEntityAttribute("OrderLine", "Quantity");
        }
    }
}
