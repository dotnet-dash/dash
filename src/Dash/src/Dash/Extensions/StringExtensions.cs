using System;

namespace Dash.Extensions
{
    public static class StringExtensions
    {
        public static bool IsSame(this string s, string? other)
        {
            return string.Equals(s, other, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsValidUri(this string s)
        {
            return s.ToUri() != null;
        }

        public static Uri? ToUri(this string s)
        {
            try
            {
                return new Uri(s);
            }
            catch (UriFormatException)
            {
                return null;
            }
        }
    }
}
