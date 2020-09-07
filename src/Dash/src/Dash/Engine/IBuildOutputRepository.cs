// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace Dash.Engine
{
    public interface IBuildOutputRepository
    {
        void Add(string path, string content);

        void Update(BuildOutput item);

        IEnumerable<BuildOutput> GetOutputItems();
    }
}