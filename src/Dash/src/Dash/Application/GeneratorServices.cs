using Dash.Engine.Abstractions;
using Dash.Engine.Generator;
using Microsoft.Extensions.DependencyInjection;

namespace Dash.Application
{
    public static class GeneratorServices
    {
        public static void Add(IServiceCollection services)
        {
            services.AddSingleton<IGenerator, DefaultGenerator>();
        }
    }
}