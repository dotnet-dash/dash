﻿// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using Dash.Engine;
using Dash.Nodes;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Nodes
{
    public class HasManyReferenceDeclarationNodeTests
    {
        [Fact]
        public async Task Accept_Visitor_VisitorShouldHaveCalledVisit()
        {
            // Arrange
            var sut = new HasManyReferenceDeclarationNode(new EntityDeclarationNode(new ModelNode(), "Parent"), "Child", "Person");

            var visitor = Substitute.For<INodeVisitor>();

            // Act
            await sut.Accept(visitor);

            // Assert
            await visitor.Received(1).Visit(sut);
        }
    }
}