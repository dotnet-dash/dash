using Microsoft.Extensions.DependencyInjection;

namespace Dash.Application
{
    public static class ApplicationServices
    {
        public static void Add(IServiceCollection services, bool verbose, string? outputDirectory)
        {
            services.AddSingleton<DashApplication>();
            services.Configure<DashOptions>(options =>
            {
                options.Verbose = verbose;
                options.OutputDirectory = outputDirectory;
            });
        }
    }
}