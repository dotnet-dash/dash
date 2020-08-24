// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.IO;
using Dash.Application;

namespace Dash.Extensions
{
    public static class UriExtensions
    {
        public static string ToPath(this Uri uri, DashOptions options)
        {
            if (uri.IsAbsoluteUri)
            {
                return uri.AbsolutePath;
            }

            return Path.Combine(options.WorkingDirectory, uri.ToString()).NormalizeSlashes();
        }
    }
}
