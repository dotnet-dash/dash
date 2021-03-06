﻿// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.IO;
using System.Linq;

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

        public static string NormalizeSlashes(this string s)
        {
            var path = s.Replace('\\', '/');
            if (path.EndsWith('/'))
            {
                path = path.TrimEnd('/') + '/';
            }

            return path;
        }

        public static string AbsolutePath(this string path)
        {
            if (!Path.IsPathRooted(path))
            {
                throw new InvalidOperationException();
            }

            return Path.GetFullPath(path).NormalizeSlashes();
        }

        public static string StartWithCapitalLetter(this string s)
        {
            if (s.Length == 0)
            {
                return string.Empty;
            }

            return s.First().ToString().ToUpper() + s.Substring(1);
        }

        public static string AppendFilenameSuffix(this string s, string suffix)
        {
            if (!string.IsNullOrWhiteSpace(suffix))
            {
                return s + "." + suffix.Trim('.');
            }

            return s;
        }
    }
}
