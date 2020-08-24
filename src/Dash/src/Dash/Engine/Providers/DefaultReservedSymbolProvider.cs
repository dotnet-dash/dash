// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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
