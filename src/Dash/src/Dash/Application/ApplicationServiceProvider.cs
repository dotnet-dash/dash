// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Dash.Application
{
    public class ApplicationServiceProvider : IApplicationServiceProvider
    {
        public IServiceCollection CreateServiceCollection(bool verbose, string workingDir)
        {
            var services = new ServiceCollection();

            ApplicationServices.Add(services, verbose, workingDir);
            CommonServices.Add(services);
            RepositoryServices.Add(services);
            SourceCodeServices.Add(services);
            NodeVisitorServices.Add(services);
            LanguageProviderServices.Add(services);
            GeneratorServices.Add(services);

            return services;
        }
    }
}