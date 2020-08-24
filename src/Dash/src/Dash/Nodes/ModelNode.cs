// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Dash.Engine;

namespace Dash.Nodes
{
    public class ModelNode : AstNode
    {
        public IList<EntityDeclarationNode> EntityDeclarations { get; } = new List<EntityDeclarationNode>();

        public override async Task Accept(INodeVisitor visitor)
        {
            await visitor.Visit(this);
        }

        public EntityDeclarationNode AddEntityDeclarationNode(string name)
        {
            var node = new EntityDeclarationNode(this, name);
            EntityDeclarations.Add(node);

            return node;
        }
    }
}