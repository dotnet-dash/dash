// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Dash.Common;
using Dash.Engine;
using Dash.Exceptions;
using Dash.Nodes;

namespace Dash.Application
{
    public class DashApplication
    {
        private readonly IFileSystem _fileSystem;
        private readonly ISourceCodeParser _sourceCodeParser;
        private readonly IEnumerable<INodeVisitor> _nodeVisitors;
        private readonly IErrorRepository _errorRepository;
        private readonly IGenerator _generator;
        private readonly IEnumerable<IPostGenerator> _postGenerators;
        private readonly IConsole _console;

        public DashApplication(
            IFileSystem fileSystem,
            ISourceCodeParser sourceCodeParser,
            IEnumerable<INodeVisitor> nodeVisitors,
            IErrorRepository errorRepository,
            IGenerator generator,
            IEnumerable<IPostGenerator> postGenerators,
            IConsole console)
        {
            _fileSystem = fileSystem;
            _sourceCodeParser = sourceCodeParser;
            _nodeVisitors = nodeVisitors;
            _errorRepository = errorRepository;
            _generator = generator;
            _postGenerators = postGenerators;
            _console = console;
        }

        public async Task Run(string? inputFile)
        {
            if (inputFile == null)
            {
                _console.Error("Please specify a model file.");
                return;
            }

            if (!_fileSystem.File.Exists(inputFile))
            {
                _console.Error($"Could not find the model file '{inputFile}'.");
                return;
            }

            var fileStream = _fileSystem.File.OpenText(inputFile);
            var sourceCode = await fileStream.ReadToEndAsync();

            try
            {
                var sourceCodeNode = _sourceCodeParser.Parse(sourceCode);
                await RunVisitors(sourceCodeNode);
            }
            catch (ParserException exception)
            {
                _console.Error($"Error while parsing the source code: {exception.Message}");
            }
        }

        private async Task RunVisitors(SourceCodeNode sourceCodeNode)
        {
            foreach (var visitor in _nodeVisitors)
            {
                _console.Trace($"Running {visitor.GetType()}");
                await visitor.Visit(sourceCodeNode);

                if (_errorRepository.HasErrors())
                {
                    _console.Error("Error(s) found:");

                    var errors = string.Join(Environment.NewLine, _errorRepository.GetErrors().Select(e => e));
                    _console.Error(errors);

                    return;
                }
            }

            await RunGenerators(sourceCodeNode);
        }

        private async Task RunGenerators(SourceCodeNode sourceCodeNode)
        {
            await _generator.Generate(sourceCodeNode);

            foreach (var postGenerator in _postGenerators)
            {
                await postGenerator.Run();
            }
        }
    }
}