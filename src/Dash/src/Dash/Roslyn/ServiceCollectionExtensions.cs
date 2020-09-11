// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Dash.Roslyn.Default;
using Microsoft.Extensions.DependencyInjection;

namespace Dash.Roslyn
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRoslynFacade(this IServiceCollection services)
        {
            services.AddSingleton<IWorkspace, BuildWorkspace>();
        }
    }
}
