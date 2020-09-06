// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.IO.Abstractions;
using System.Threading.Tasks;
using Dash.Common;
using Dash.Engine;
using Dash.Exceptions;
using Microsoft.Extensions.Options;

namespace Dash.Application
{
    public class DashApplication
    {
        private readonly IConsole _console;
        private readonly IDashOptionsValidator _dashOptionsValidator;
        private readonly DashOptions _dashOptions;
        private readonly IFileSystem _fileSystem;
        private readonly ISourceCodeParser _sourceCodeParser;
        private readonly ISourceCodeProcessor _sourceCodeProcessor;

        public DashApplication(
            IConsole console,
            IDashOptionsValidator dashOptionsValidator,
            IOptions<DashOptions> dashOptions,
            IFileSystem fileSystem,
            ISourceCodeParser sourceCodeParser,
            ISourceCodeProcessor sourceCodeProcessor)
        {
            _console = console;
            _dashOptionsValidator = dashOptionsValidator;
            _dashOptions = dashOptions.Value;
            _fileSystem = fileSystem;
            _sourceCodeParser = sourceCodeParser;
            _sourceCodeProcessor = sourceCodeProcessor;
        }

        public async Task Run()
        {
            var isValid = await _dashOptionsValidator.Validate();
            if (isValid)
            {
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
}