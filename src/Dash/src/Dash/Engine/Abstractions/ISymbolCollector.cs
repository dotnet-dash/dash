using System.Collections.Generic;

namespace Dash.Engine.Abstractions
{
    public interface ISymbolCollector : INodeVisitor
    {
        HashSet<string> GetEntityNames();

        HashSet<string> GetAttributeNames(string entityName);
    }
}
