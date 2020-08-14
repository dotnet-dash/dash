using System;
using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Dash.Application;
using Dash.Engine;
using Dash.Engine.Abstractions;
using Dash.Engine.Generator;
using Dash.Engine.JsonParser;
using Dash.Engine.LanguageProviders;
using Dash.Engine.Template;
using Microsoft.Extensions.DependencyInjection;

namespace Dash
{
    class Program
    {
        static async Task Main(FileInfo file, DirectoryInfo output, bool verbose = false)
        {
            if (file == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine("Please specify a model file.");
                Console.ResetColor();
                return;
            }

            if (!file.Exists)
            {
                Console.Error.WriteLine($"The specified input {file} not found");
                return;
            }

            if (!output.Exists)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"The specified output location '{output}' does not exist.");
                Console.ResetColor();
                return;
            }

            var services = RegisterServices(output, verbose);
            using var scope = services.CreateScope();
            var app = scope.ServiceProvider.GetRequiredService<DashApplication>();
            await app.Run(file);
        }

        private static ServiceProvider RegisterServices(DirectoryInfo output, bool verbose)
        {
            var services = new ServiceCollection();
            services.AddSingleton<DashApplication>();
            services.Configure<DashOptions>(options =>
            {
                options.OutputDirectory = output.ToString();

                options.Verbose = verbose;
            });
            services.AddSingleton<IGenerator, DefaultGenerator>();
            services.AddSingleton<ISemanticAnalyzer, DefaultSemanticAnalyzer>();
            services.AddSingleton<IParser, JsonParser>();
            services.AddSingleton<ITemplateProvider, EmbeddedTemplateProvider>();
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<ILanguageProvider, CSharpLanguageProvider>();
            services.AddSingleton<ILanguageProvider, SqlServerLanguageProvider>();
            services.AddSingleton<DataTypeParser>();

            return services.BuildServiceProvider(true);
        }
    }
}
