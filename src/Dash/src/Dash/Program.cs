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
        static async Task Main(FileInfo inputFile, string[] templates, bool verbose = false)
        {
            //Console.WriteLine($"The value for --input-file is: {inputFile}");
            //Console.WriteLine($"The value for --templates is: {string.Join(";", templates.Select(e => e))}");

            if (inputFile == null)
            {
                Console.Error.WriteLine("No input file defined");
                return;
            }

            if (inputFile.Exists == false)
            {
                Console.Error.WriteLine($"{inputFile} not found");
                return;
            }

            var services = RegisterServices(templates, verbose);
            using var scope = services.CreateScope();
            var app = scope.ServiceProvider.GetRequiredService<DashApplication>();
            await app.Run(inputFile);
        }

        private static ServiceProvider RegisterServices(string[] templates, bool verbose)
        {
            var services = new ServiceCollection();
            services.AddSingleton<DashApplication>();
            services.Configure<DashOptions>(options =>
            {
                if (templates?.Length > 0)
                {
                    options.Templates = templates;
                }

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
