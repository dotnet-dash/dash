// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

namespace Dash.Engine.Repositories
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IModelRepository, DefaultModelRepository>();
            services.AddSingleton<ISymbolRepository, DefaultSymbolRepository>();
            services.AddSingleton<IErrorRepository, ErrorRepository>();
            services.AddSingleton<IUriResourceRepository, UriResourceRepository>();
            services.AddSingleton<IBuildOutputRepository, BuildOutputRepository>();
        }
    }
}
