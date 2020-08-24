// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.IO.Abstractions;
using Dash.Common;
using Dash.Common.Default;
using Dash.Engine;
using Dash.Engine.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Dash.Application
{
    public static class CommonServices
    {
        public static void Add(IServiceCollection services)
        {
            services.AddSingleton<IClock, Clock>();
            services.AddSingleton<IConsole, DefaultConsole>();
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<ISessionService, SessionService>();
            services.AddHttpClient();

            services.AddSingleton<IEmbeddedTemplateProvider, EmbeddedTemplateProvider>();
            services.AddSingleton<IHttpUriDownloader, HttpUriDownloader>();
        }
    }
}
