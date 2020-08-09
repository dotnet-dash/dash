using System;

namespace Dash.Extensions
{
    public static class StringExtensions
    {
        public static bool IsSame(this string s, string? other)
        {
            return string.Equals(s, other, StringComparison.OrdinalIgnoreCase);
        }
    }
}
