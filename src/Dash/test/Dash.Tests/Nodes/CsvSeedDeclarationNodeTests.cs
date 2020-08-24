// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dash.Engine;
using Dash.Nodes;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Nodes
{
    public class CsvSeedDeclarationNodeTests
    {
        [Fact]
        public async Task Accept_Visitor_SutShouldHaveCalledVisit()
        {
            // Arrange
            var node = new EntityDeclarationNode(new ModelNode(), "Foo");
            var sut = new CsvSeedDeclarationNode(node, new Uri("https://foo"), false, null, new Dictionary<string, string>());

            var visitor = Substitute.For<INodeVisitor>();

            // Act
            await sut.Accept(visitor);

            // Assert
            await visitor.Received(1).Visit(sut);
        }
    }
}