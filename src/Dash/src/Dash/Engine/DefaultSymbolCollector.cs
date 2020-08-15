using System;
using System.Collections.Generic;
using System.Linq;
using Dash.Engine.Abstractions;
using Dash.Extensions;
using Dash.Nodes;

namespace Dash.Engine
{
    public class DefaultSymbolCollector : ISymbolCollector
    {
        private readonly Dictionary<string, HashSet<string>> _allEntities = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);

        public void Visit(ModelNode node)
        {
            _allEntities.Clear();

            foreach (var entityDeclarationNode in node.EntityDeclarations)
            {
                entityDeclarationNode.Accept(this);
            }
        }

        public void Visit(EntityDeclarationNode node)
        {
            _allEntities.TryAdd(node.Name, new HashSet<string>());
        }

        public void Visit(AttributeDeclarationNode node)
        {
            if (_allEntities.TryGetValue(node.Parent.Name, out var attributeHashSet))
            {
                attributeHashSet.Add(node.AttributeName);
            }
        }

        public void Visit(ReferenceDeclarationNode node)
        {
        }

        public void Visit(HasReferenceDeclarationNode node)
        {
        }

        public void Visit(HasManyReferenceDeclarationNode node)
        {
        }

        public void Visit(HasAndBelongsToManyDeclarationNode node)
        {
        }

        public HashSet<string> GetEntityNames()
        {
            return _allEntities.Select(e => e.Key).ToHashSet();
        }

        public HashSet<string> GetAttributeNames(string entityName)
        {
            return _allEntities.TryGetValue(entityName, out var result)
                ? result
                : new HashSet<string>();
        }

        public bool EntityExists(string entityName)
        {
            var entity = GetEntityNames().FirstOrDefault(e => e.IsSame(entityName));
            return entity != null;
        }
    }
}
