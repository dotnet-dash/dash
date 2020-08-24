// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Dash.Engine;
using Dash.Engine.LanguageProviders;
using Microsoft.Extensions.DependencyInjection;

namespace Dash.Application
{
    public static class LanguageProviderServices
    {
        public static void Add(IServiceCollection services)
        {
            services.AddSingleton<ILanguageProvider, CSharpLanguageProvider>();
            services.AddSingleton<ILanguageProvider, SqlServerLanguageProvider>();
        }
    }
}
