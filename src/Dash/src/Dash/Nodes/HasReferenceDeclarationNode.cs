// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using Dash.Engine;

namespace Dash.Nodes
{
    public class HasReferenceDeclarationNode : ReferenceDeclarationNode
    {
        public HasReferenceDeclarationNode(EntityDeclarationNode parent, string name, string referencedEntity) :
            base(parent, name, referencedEntity)
        {
        }

        public override async Task Accept(INodeVisitor visitor)
        {
            await visitor.Visit(this);
        }
    }
}