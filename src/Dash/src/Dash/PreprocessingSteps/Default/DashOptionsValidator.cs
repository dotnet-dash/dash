// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.IO.Abstractions;
using System.Threading.Tasks;
using Dash.Application;
using Dash.Common;
using Microsoft.Extensions.Options;

namespace Dash.PreprocessingSteps.Default
{
    public class DashOptionsValidator : IPreprocessingStep
    {
        private readonly IConsole _console;
        private readonly IFileSystem _fileSystem;
        private readonly DashOptions _dashOptions;

        public DashOptionsValidator(
            IConsole console,
            IFileSystem fileSystem,
            IOptions<DashOptions> dashOptions)
        {
            _console = console;
            _fileSystem = fileSystem;
            _dashOptions = dashOptions.Value;
        }

        public Task<bool> Process()
        {
            if (_dashOptions.InputFile == null)
            {
                _console.Error("Please specify a model file.");
                return Task.FromResult(false);
            }

            if (!_fileSystem.File.Exists(_dashOptions.InputFile))
            {
                _console.Error($"Could not find the model file '{_dashOptions.InputFile}'.");
                return Task.FromResult(false);
            }

            if (_dashOptions.Project != null && !_fileSystem.File.Exists(_dashOptions.Project))
            {
                _console.Error($"Could not find the .csproj file '{_dashOptions.Project}'.");
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}
