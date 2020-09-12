// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Dash.Common;
using Dash.Engine.Repositories;
using Dash.Engine.Visitors;
using Dash.PreprocessingSteps;
using Dash.Roslyn;
using Microsoft.Extensions.DependencyInjection;

namespace Dash.Application.Default
{
    public class Startup : IStartup
    {
        public IServiceCollection ConfigureServices(DashOptions dashOptions)
        {
            var services = new ServiceCollection();

            ApplicationServices.Add(services, dashOptions);
            SourceCodeServices.Add(services);
            LanguageProviderServices.Add(services);
            GeneratorServices.Add(services);

            services.AddHttpClient();
            services.AddCommonServices();
            services.AddPreprocessingSteps();
            services.AddRoslynFacade();
            services.AddRepositories();
            services.AddNodeVisitors();

            return services;
        }
    }
}