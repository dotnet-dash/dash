using System.Collections.Generic;

namespace Dash.Engine
{
    public interface ISymbolRepository
    {
        void AddEntity(string entityName);

        void AddEntityAttribute(string entityName, string attributeName);

        HashSet<string> GetEntityNames();

        HashSet<string> GetAttributeNames(string entityName);

        bool EntityExists(string entityName);
    }
}
