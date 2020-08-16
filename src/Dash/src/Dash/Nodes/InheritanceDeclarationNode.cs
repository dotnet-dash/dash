﻿using Dash.Engine.Abstractions;

namespace Dash.Nodes
{
    public class InheritanceDeclarationNode : AstNode
    {
        public InheritanceDeclarationNode(EntityDeclarationNode parent, string inheritedEntity)
        {
            Parent = parent;
            InheritedEntity = inheritedEntity;
        }

        public EntityDeclarationNode Parent { get; set; }

        public string InheritedEntity { get; set; }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}