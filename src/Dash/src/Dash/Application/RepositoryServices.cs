using Dash.Engine.Abstractions;
using Dash.Engine.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Dash.Application
{
    public static class RepositoryServices
    {
        public static void Add(IServiceCollection services)
        {
            services.AddSingleton<IModelRepository, DefaultModelRepository>();
            services.AddSingleton<ISymbolRepository, DefaultSymbolRepository>();
            services.AddSingleton<IErrorRepository, ErrorRepository>();
            services.AddSingleton<IUriResourceRepository, UriResourceRepository>();
        }
    }
}
