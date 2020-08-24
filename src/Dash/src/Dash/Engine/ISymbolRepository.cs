// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace Dash.Engine
{
    public interface ISymbolRepository
    {
        void AddEntity(string entityName);

        void AddEntityAttribute(string entityName, string attributeName);

        HashSet<string> GetEntityNames();

        HashSet<string> GetAttributeNames(string entityName);

        bool EntityExists(string entityName);
    }
}
