using Microsoft.Extensions.DependencyInjection;

namespace Dash.Engine.Repositories
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IModelRepository, DefaultModelRepository>();
            services.AddSingleton<ISymbolRepository, DefaultSymbolRepository>();
            services.AddSingleton<IErrorRepository, ErrorRepository>();
            services.AddSingleton<IUriResourceRepository, UriResourceRepository>();
            services.AddSingleton<IBuildOutputRepository, BuildOutputRepository>();
        }
    }
}
