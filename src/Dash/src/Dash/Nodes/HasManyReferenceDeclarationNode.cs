﻿using System.Threading.Tasks;
using Dash.Engine;

namespace Dash.Nodes
{
    public class HasManyReferenceDeclarationNode : ReferenceDeclarationNode
    {
        public HasManyReferenceDeclarationNode(EntityDeclarationNode parent, string name, string referencedEntity) :
            base(parent, name, referencedEntity)
        {
        }

        public override async Task Accept(INodeVisitor visitor)
        {
            await visitor.Visit(this);
        }
    }
}