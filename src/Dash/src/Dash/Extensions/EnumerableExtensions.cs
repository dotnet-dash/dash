using System;
using System.Collections.Generic;
using System.Linq;

namespace Dash.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool Has(this IEnumerable<string> enumerable, string value)
        {
            return enumerable.Contains(value, StringComparer.OrdinalIgnoreCase);
        }
    }
}
