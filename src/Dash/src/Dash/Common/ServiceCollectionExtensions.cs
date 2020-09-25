// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.IO.Abstractions;
using Dash.Common.Default;
using Dash.Engine;
using Dash.Engine.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Dash.Common
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection services)
        {
            services.AddSingleton<IClock, Clock>();
            services.AddSingleton<IConsole, DefaultConsole>();
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<ISessionService, SessionService>();

            services.AddSingleton<IEmbeddedTemplateProvider, EmbeddedTemplateProvider>();
            services.AddSingleton<IHttpUriDownloader, HttpUriDownloader>();
        }
    }
}
