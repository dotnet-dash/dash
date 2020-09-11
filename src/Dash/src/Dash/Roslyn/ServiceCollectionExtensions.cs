using Dash.Roslyn.Default;
using Microsoft.Extensions.DependencyInjection;

namespace Dash.Roslyn
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRoslynFacade(this IServiceCollection services)
        {
            services.AddSingleton<IWorkspace, BuildWorkspace>();
        }
    }
}
