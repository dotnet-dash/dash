// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Dash.Nodes
{
    public abstract class ReferenceDeclarationNode : AstNode
    {
        protected ReferenceDeclarationNode(EntityDeclarationNode parent, string name, string referencedEntity)
        {
            Parent = parent;
            Name = name;
            ReferencedEntity = referencedEntity;
        }

        public EntityDeclarationNode Parent { get; }

        public string Name { get; }

        public string ReferencedEntity { get; }
    }
}