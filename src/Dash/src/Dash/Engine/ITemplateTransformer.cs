// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Dash.Engine.Models;

namespace Dash.Engine
{
    public interface ITemplateTransformer
    {
        Task<string> Transform(string templateText, IEnumerable<EntityModel> entities);
    }
}
