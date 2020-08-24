// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using Dash.Common;
using Dash.Nodes;

namespace Dash.Engine.Visitors
{
    public class CreateJoinedEntityVisitor : BaseVisitor
    {
        public CreateJoinedEntityVisitor(IConsole console) : base(console)
        {
        }

        public override Task Visit(HasAndBelongsToManyDeclarationNode node)
        {
            var joinedEntityName = node.Parent.Name + node.ReferencedEntity;
            Console.Trace($"Adding joined entity: {joinedEntityName}");

            var joinedEntity = node.Parent.Parent.AddEntityDeclarationNode(joinedEntityName);
            joinedEntity.AddHasDeclaration(node.Parent.Name, node.Parent.Name);
            joinedEntity.AddHasDeclaration(node.ReferencedEntity, node.ReferencedEntity);

            return base.Visit(node);
        }
    }
}
