using System;
using System.Collections.Generic;
using System.Linq;
using Dash.Extensions;

namespace Dash.Engine.Repositories
{
    public class DefaultSymbolRepository : ISymbolRepository
    {
        private readonly Dictionary<string, HashSet<string>> _symbols = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);

        public void AddEntity(string entityName)
        {
            _symbols.TryAdd(entityName, new HashSet<string>());
        }

        public void AddEntityAttribute(string entityName, string attributeName)
        {
            if (_symbols.TryGetValue(entityName, out var attributeHashSet))
            {
                attributeHashSet.Add(attributeName);
            }
        }

        public HashSet<string> GetEntityNames()
        {
            return _symbols.Select(e => e.Key).ToHashSet();
        }

        public HashSet<string> GetAttributeNames(string entityName)
        {
            return _symbols.TryGetValue(entityName, out var result)
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
