// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace Dash.Engine
{
    public interface IErrorRepository
    {
        void Add(string error);
        bool HasErrors();
        IEnumerable<string> GetErrors();
    }
}
