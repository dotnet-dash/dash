// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using Dash.Engine;

namespace Dash.Nodes
{
    public class AttributeDeclarationNode : AstNode
    {
        public AttributeDeclarationNode(EntityDeclarationNode parent, string attributeName, string attributeDataType)
        {
            Parent = parent;
            AttributeName = attributeName;
            AttributeDataType = attributeDataType;
        }

        public EntityDeclarationNode Parent { get; set; }

        public string AttributeName { get; set; }

        public string AttributeDataType { get; set; }

        public override async Task Accept(INodeVisitor visitor)
        {
            await visitor.Visit(this);
        }
    }
}