using Dash.Engine.Abstractions;
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
