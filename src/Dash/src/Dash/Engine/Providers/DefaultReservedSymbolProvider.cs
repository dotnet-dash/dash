using System;
using System.Collections.Generic;

namespace Dash.Engine.Providers
{
    public class DefaultReservedSymbolProvider : IReservedSymbolProvider
    {
        private readonly HashSet<string> _reservedKeywords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "object"
        };

        public bool IsReservedEntityName(string keyword)
        {
            return _reservedKeywords.Contains(keyword);
        }
    }
}
