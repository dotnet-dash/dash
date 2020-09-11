// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Dash.Common;
using Dash.Engine;
using Dash.Exceptions;
using Dash.PreprocessingSteps;
using Microsoft.Extensions.Options;

namespace Dash.Application
{
    public class DashApplication
    {
        private readonly IConsole _console;
        private readonly IEnumerable<IPreprocessingStep> _preprocessingSteps;
        private readonly DashOptions _dashOptions;
        private readonly IFileSystem _fileSystem;
        private readonly ISourceCodeParser _sourceCodeParser;
        private readonly ISourceCodeProcessor _sourceCodeProcessor;

        public DashApplication(
            IConsole console,
            IEnumerable<IPreprocessingStep> preprocessingSteps,
            IOptions<DashOptions> dashOptions,
            IFileSystem fileSystem,
            ISourceCodeParser sourceCodeParser,
            ISourceCodeProcessor sourceCodeProcessor)
        {
            _console = console;
            _preprocessingSteps = preprocessingSteps;
            _dashOptions = dashOptions.Value;
            _fileSystem = fileSystem;
            _sourceCodeParser = sourceCodeParser;
            _sourceCodeProcessor = sourceCodeProcessor;
        }

        public async Task Run()
        {
            foreach (var step in _preprocessingSteps)
            {
                var succeeded = await step.Process();
                if (!succeeded)
                {
                    return;
                }
            }

            var fileStream = _fileSystem.File.OpenText(_dashOptions.InputFile);
            var sourceCode = await fileStream.ReadToEndAsync();

            try
            {
                var sourceCodeNode = _sourceCodeParser.Parse(sourceCode);
                await _sourceCodeProcessor.WalkTree(sourceCodeNode);
            }
            catch (ParserException exception)
            {
                _console.Error($"Error while parsing the source code: {exception.Message}");
            }
        }
    }
}