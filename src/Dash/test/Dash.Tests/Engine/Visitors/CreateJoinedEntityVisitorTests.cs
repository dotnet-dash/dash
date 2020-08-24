// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using Dash.Common;
using Dash.Engine.Visitors;
using Dash.Nodes;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Engine.Visitors
{
    public class CreateJoinedEntityVisitorTests
    {
        [Fact]
        public async Task Visit_HasAndBelongsToManyDeclarationNode()
        {
            // Arrange
            var sut = new CreateJoinedEntityVisitor(Substitute.For<IConsole>());

            var modelNode = new ModelNode();
            HasAndBelongsToManyDeclarationNode node = modelNode
                .AddEntityDeclarationNode("Foo")
                .AddHasAndBelongsToManyDeclarationNode("Bar", "Bar")
                .HasAndBelongsToMany
                .Last();

            // Act
            await sut.Visit(node);

            // Assert
            modelNode.EntityDeclarations.Should().SatisfyRespectively(
                first => first.Name.Should().Be("Foo"),
                second => second.Name.Should().Be("FooBar"));
        }
    }
}
