// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Dash.Application.Default;
using Microsoft.Extensions.DependencyInjection;

namespace Dash.Application
{
    public static class ApplicationServices
    {
        public static void Add(IServiceCollection services, DashOptions dashOptions)
        {
            services.AddSingleton<DashApplication>();
            services.Configure<DashOptions>(options =>
            {
                options.InputFile = dashOptions.InputFile;
                options.Project = dashOptions.Project;
                options.Verbose = dashOptions.Verbose;
            });

            services.AddSingleton<ISourceCodeProcessor, SourceCodeProcessor>();
        }
    }
}