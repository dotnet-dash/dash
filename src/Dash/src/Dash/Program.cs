using System.IO;
using System.Threading.Tasks;
using Dash.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Dash
{
    public class Program
    {
        public static async Task Main(FileInfo file, bool verbose = false)
        {
            var applicationServiceProvider = new ApplicationServiceProvider();

            var services = applicationServiceProvider.Create(verbose);
            using var scope = services.CreateScope();
            var app = scope.ServiceProvider.GetRequiredService<DashApplication>();
            await app.Run(file, verbose);
        }
    }
}
