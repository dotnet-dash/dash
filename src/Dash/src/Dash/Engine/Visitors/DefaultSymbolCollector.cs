using System;
using System.Collections.Generic;
using System.Linq;
using Dash.Engine.Abstractions;
using Dash.Extensions;
using Dash.Nodes;

namespace Dash.Engine.Visitors
{
    public class DefaultSymbolCollector : BaseVisitor, ISymbolCollector
    {
        private readonly IConsole _console;
        private readonly Dictionary<string, HashSet<string>> _allEntities = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);

        public DefaultSymbolCollector(IConsole console)
        {
            _console = console;
        }

        public override void Visit(ModelNode node)
        {
            _allEntities.Clear();

            base.Visit(node);
        }

        public override void Visit(EntityDeclarationNode node)
        {
            _allEntities.TryAdd(node.Name, new HashSet<string>());
            _console.WriteLine($"Adding symbol: {node.Name}");

            base.Visit(node);
        }

        public override void Visit(AttributeDeclarationNode node)
        {
            if (_allEntities.TryGetValue(node.Parent.Name, out var attributeHashSet))
            {
                attributeHashSet.Add(node.AttributeName);
            }

            base.Visit(node);
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
