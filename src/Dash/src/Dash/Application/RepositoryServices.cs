// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Dash.Engine;
using Dash.Engine.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Dash.Application
{
    public static class RepositoryServices
    {
        public static void Add(IServiceCollection services)
        {
            services.AddSingleton<IModelRepository, DefaultModelRepository>();
            services.AddSingleton<ISymbolRepository, DefaultSymbolRepository>();
            services.AddSingleton<IErrorRepository, ErrorRepository>();
            services.AddSingleton<IUriResourceRepository, UriResourceRepository>();
            services.AddSingleton<IBuildOutputRepository, BuildOutputRepository>();
        }
    }
}
