using System.IO;
using System.Threading.Tasks;
using Dash.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Dash
{
    public static class Program
    {
        public static async Task Main(FileInfo? file, string? outputDir = null, bool verbose = false)
        {
            IApplicationServiceProvider applicationServiceProvider = new ApplicationServiceProvider();

            var services = applicationServiceProvider.Create(verbose, outputDir);
            using var scope = services.CreateScope();
            var app = scope.ServiceProvider.GetRequiredService<DashApplication>();
            await app.Run(file);
        }
    }
}
