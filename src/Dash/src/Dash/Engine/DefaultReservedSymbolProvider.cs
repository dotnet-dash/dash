using System;
using System.Collections.Generic;
using Dash.Engine.Abstractions;

namespace Dash.Engine
{
    public class DefaultReservedSymbolProvider : IReservedSymbolProvider
    {
        private readonly HashSet<string> _reservedKeywords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "base",
            "object"
        };

        public bool IsReservedEntityName(string keyword)
        {
            return _reservedKeywords.Contains(keyword);
        }
    }
}
