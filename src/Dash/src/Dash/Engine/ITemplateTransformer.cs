﻿// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;

namespace Dash.Engine
{
    public interface ITemplateTransformer
    {
        Task<string> Transform(string templateText);
    }
}
