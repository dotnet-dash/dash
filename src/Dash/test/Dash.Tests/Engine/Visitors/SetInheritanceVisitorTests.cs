// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using Dash.Engine.Visitors;
using Dash.Nodes;
using FluentArrange.NSubstitute;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Engine.Visitors
{
    public class SetInheritanceVisitorTests
    {
        [Fact]
        public async Task Visit_ModelNode_ShouldHaveSetDefaultInheritanceIfNotDeclared()
        {
            // Arrange
            var sut = Arrange.Sut<SetInheritanceVisitor>();

            var modelNode = new ModelNode();
            modelNode.AddEntityDeclarationNode("Order");
            modelNode.AddEntityDeclarationNode("OrderLine").AddInheritanceDeclaration("SomeOtherBase");

            // Act
            await sut.Visit(modelNode);

            // Assert
            modelNode.EntityDeclarations.Should().SatisfyRespectively(
                first =>
                {
                    first.Name.Should().Be("Order");
                    first.InheritanceDeclarationNodes.Should().SatisfyRespectively(a => a.InheritedEntity.Should().Be("Base"));
                },
                second =>
                {
                    second.Name.Should().Be("OrderLine");
                    second.InheritanceDeclarationNodes.Should().SatisfyRespectively(a => a.InheritedEntity.Should().Be("SomeOtherBase"));
                },
                third =>
                {
                    third.Name.Should().Be("Base");
                    third.InheritanceDeclarationNodes.Should().BeEmpty();
                });
        }
    }
}
