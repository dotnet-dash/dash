﻿// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Dash.Application;
using Dash.Common;
using Microsoft.Extensions.Options;

namespace Dash.PreprocessingSteps.Default
{
    public class FindProjectFile : IPreprocessingStep
    {
        private readonly IConsole _console;
        private readonly DashOptions _options;
        private readonly IFileSystem _fileSystem;

        public FindProjectFile(
            IOptions<DashOptions> options,
            IConsole console,
            IFileSystem fileSystem)
        {
            _options = options.Value;
            _console = console;
            _fileSystem = fileSystem;
        }

        public Task<bool> Process()
        {
            if (_options.Project == null)
            {
                _console.Info("No .csproj specified. Finding .csproj");

                var path = _fileSystem.Directory.GetCurrentDirectory();

                var projectFiles = _fileSystem.Directory.GetFiles(path, "*.csproj");
                if (projectFiles.Length == 0)
                {
                    _console.Error("No .csproj file found in working directory.");
                    return Task.FromResult(false);
                }

                if (projectFiles.Length > 1)
                {
                    _console.Error("Multiple .csproj files found in working directory. Please specify the project explicitly.");
                    return Task.FromResult(false);
                }

                _options.Project = projectFiles.First();
                return Task.FromResult(true);
            }

            return Task.FromResult(true);
        }
    }
}
