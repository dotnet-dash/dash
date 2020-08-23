using System;
using Dash.Engine;
using Dash.Engine.Abstractions;
using Dash.Engine.Generator;
using Dash.Engine.TemplateProviders;
using Microsoft.Extensions.DependencyInjection;

namespace Dash.Application
{
    public class ApplicationServiceProvider : IApplicationServiceProvider
    {
        public IServiceProvider Create(bool verbose)
        {
            var services = new ServiceCollection();
            services.Configure<DashOptions>(options =>
            {
                options.Verbose = verbose;
            });

            services.AddSingleton<DashApplication>();
            services.AddSingleton<IGenerator, DefaultGenerator>();
            services.AddSingleton<IEmbeddedTemplateProvider, EmbeddedTemplateProvider>();
            services.AddSingleton<IDownloadHttpResource, DownloadHttpResource>();

            CommonServices.Add(services);
            NodeVisitorServices.Add(services);
            RepositoryServices.Add(services);
            LanguageProviderServices.Add(services);
            SourceCodeServices.Add(services);

            return services.BuildServiceProvider(true);
        }
    }
}
