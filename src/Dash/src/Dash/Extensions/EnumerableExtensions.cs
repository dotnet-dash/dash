using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dash.Engine;
using Dash.Nodes;

namespace Dash.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool Has(this IEnumerable<string> enumerable, string value)
        {
            return enumerable.Contains(value, StringComparer.OrdinalIgnoreCase);
        }

        public static async Task Accept<T>(this IEnumerable<T> nodes, INodeVisitor visitor) where T : AstNode
        {
            foreach (var entityDeclaration in nodes.ToList())
            {
                await entityDeclaration.Accept(visitor);
            }
        }

        public static Task Accept<T>(this IList<T> nodes, INodeVisitor visitor) where T : AstNode => Accept(nodes.AsEnumerable(), visitor);
    }
}