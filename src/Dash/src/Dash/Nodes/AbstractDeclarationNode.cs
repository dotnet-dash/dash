// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using Dash.Engine;

namespace Dash.Nodes
{
    public class AbstractDeclarationNode : AstNode
    {
        public AbstractDeclarationNode(EntityDeclarationNode parent, bool value)
        {
            Parent = parent;
            Value = value;
        }

        public EntityDeclarationNode Parent { get; }

        public bool Value { get; }

        public override async Task Accept(INodeVisitor visitor)
        {
            await visitor.Visit(this);
        }
    }
}
