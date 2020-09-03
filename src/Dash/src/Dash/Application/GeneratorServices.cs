﻿// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Dash.Engine;
using Dash.Engine.Generator;
using Dash.Engine.Templates;
using Microsoft.Extensions.DependencyInjection;

namespace Dash.Application
{
    public static class GeneratorServices
    {
        public static void Add(IServiceCollection services)
        {
            services.AddSingleton<ITemplateTransformer, MorestachioTemplateTransformer>();
            services.AddSingleton<IGenerator, DefaultGenerator>();
            services.AddSingleton<IPostGenerator, WriteBuildOutputPostGenerator>();
        }
    }
}