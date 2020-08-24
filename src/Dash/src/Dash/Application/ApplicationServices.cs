// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

namespace Dash.Application
{
    public static class ApplicationServices
    {
        public static void Add(IServiceCollection services, bool verbose, string workingDirectory)
        {
            services.AddSingleton<DashApplication>();
            services.Configure<DashOptions>(options =>
            {
                options.Verbose = verbose;
                options.WorkingDirectory = workingDirectory;
            });
        }
    }
}