// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using Dash.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Dash
{
    public class Program
    {
        private readonly IApplicationServiceProvider _applicationServiceProvider;

        /// <summary></summary>
        /// <param name="file">The Dash model file.</param>
        /// <param name="workingDir">Used as the base path for relative paths defined inside your Model file</param>
        /// <param name="verbose">Show verbose output</param>
        public static async Task Main(string? file, string workingDir = ".", bool verbose = false)
        {
            var program = new Program(new ApplicationServiceProvider());
            await program.Run(file, workingDir, verbose);
        }

        public Program(IApplicationServiceProvider applicationServiceProvider)
        {
            _applicationServiceProvider = applicationServiceProvider;
        }

        public async Task Run(string? file, string workingDir, bool verbose)
        {
            var services = _applicationServiceProvider
                .CreateServiceCollection(verbose, workingDir)
                .BuildServiceProvider();

            using var scope = services.CreateScope();
            var app = scope.ServiceProvider.GetRequiredService<DashApplication>();
            await app.Run(file);
        }
    }
}
