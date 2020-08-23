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
