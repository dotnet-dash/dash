// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Dash.Application;
using Dash.Application.Default;
using Microsoft.Extensions.DependencyInjection;

namespace Dash
{
    public class Program
    {
        private readonly IStartup _startup;

        /// <summary></summary>
        /// <param name="file">The Dash model file.</param>
        /// <param name="projectFile">The .csproj file. If unspecified, Dash will automatically try to find the .csproj.</param>
        /// <param name="workingDir">Used as the base path for relative paths defined inside your Model file</param>
        /// <param name="verbose">Show verbose output</param>
        [ExcludeFromCodeCoverage]
        public static async Task Main(string? file, string? projectFile, string workingDir = ".", bool verbose = false)
        {
            await new Program(new Startup())
                .Run(new DashOptions
                {
                    InputFile = file,
                    ProjectFile = projectFile,
                    WorkingDirectory = workingDir,
                    Verbose = verbose,
                });
        }

        public Program(IStartup startup)
        {
            _startup = startup;
        }

        public async Task Run(DashOptions dashOptions)
        {
            var services = _startup
                .CreateServiceCollection(dashOptions)
                .BuildServiceProvider();

            using var scope = services.CreateScope();
            await scope.ServiceProvider.GetRequiredService<DashApplication>().Run();
        }
    }
}
