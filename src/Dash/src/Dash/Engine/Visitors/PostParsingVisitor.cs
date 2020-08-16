﻿using System.Linq;
using Dash.Engine.Abstractions;
using Dash.Extensions;
using Dash.Nodes;

namespace Dash.Engine.Visitors
{
    public class PostParsingVisitor : BaseVisitor
    {
        private const string BaseEntityName = "Base";
        private const string BaseEntityIdAttributeName = "Id";
        private const string BaseEntityIdAttributeDataType = "Int";
        private readonly IConsole _console;

        public PostParsingVisitor(IConsole console)
        {
            _console = console;
        }

        public override void Visit(ModelNode node)
        {
            var entity = GetOrAddBaseEntityIfNotDeclared(node);
            AddIdAttributeIfNotDeclared(entity);

            base.Visit(node);
        }

        public override void Visit(EntityDeclarationNode node)
        {
            if (!node.InheritanceDeclarationNodes.Any())
            {
                if (!node.Name.IsSame(BaseEntityName))
                {
                    _console.WriteLine($"No custom inheritance defined for '{node.Name}', setting inheritance to '{BaseEntityName}'");
                    node.AddInheritanceDeclaration(BaseEntityName);
                }
            }

            base.Visit(node);
        }

        private EntityDeclarationNode GetOrAddBaseEntityIfNotDeclared(ModelNode node)
        {
            var baseEntity = node.EntityDeclarations.SingleOrDefault(e => e.Name.IsSame(BaseEntityName));
            if (baseEntity == null)
            {
                _console.WriteLine($"No entity '{BaseEntityName}' declared. Adding '{BaseEntityName}'");
                baseEntity = new EntityDeclarationNode(BaseEntityName);
                node.EntityDeclarations.Add(baseEntity);
            }

            return baseEntity;
        }

        private void AddIdAttributeIfNotDeclared(EntityDeclarationNode baseEntity)
        {
            var idAttribute = baseEntity.AttributeDeclarations.FirstOrDefault(e => e.AttributeName.IsSame(BaseEntityIdAttributeName));
            if (idAttribute == null)
            {
                _console.WriteLine($"No attribute '{BaseEntityIdAttributeName}' declared. Adding '{BaseEntityIdAttributeName}'");
                baseEntity.InsertAttributeDeclaration(0, BaseEntityIdAttributeName, BaseEntityIdAttributeDataType);
            }
        }
    }
}
