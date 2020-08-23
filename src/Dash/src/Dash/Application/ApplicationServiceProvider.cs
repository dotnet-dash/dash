using System;
using Microsoft.Extensions.DependencyInjection;

namespace Dash.Application
{
    public class ApplicationServiceProvider : IApplicationServiceProvider
    {
        public IServiceProvider Create(bool verbose)
        {
            var services = new ServiceCollection();

            ApplicationServices.Add(services, verbose);
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