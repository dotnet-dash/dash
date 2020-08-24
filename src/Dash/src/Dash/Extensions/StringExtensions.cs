// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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
                return new Uri(s, s.StartsWith(".") ? UriKind.Relative : UriKind.Absolute);
            }
            catch (UriFormatException)
            {
                return null;
            }
        }
    }
}
