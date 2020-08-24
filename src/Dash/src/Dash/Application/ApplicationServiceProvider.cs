// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Dash.Application
{
    public class ApplicationServiceProvider : IApplicationServiceProvider
    {
        public IServiceProvider Create(bool verbose, string? outputDir)
        {
            var services = new ServiceCollection();

            ApplicationServices.Add(services, verbose, outputDir);
            CommonServices.Add(services);
            RepositoryServices.Add(services);
            SourceCodeServices.Add(services);
            NodeVisitorServices.Add(services);
            LanguageProviderServices.Add(services);
            GeneratorServices.Add(services);

            return services.BuildServiceProvider(true);
        }
    }
}