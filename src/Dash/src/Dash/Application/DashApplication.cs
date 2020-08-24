// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IConsole _console;

        public DashApplication(
            IFileSystem fileSystem,
            ISourceCodeParser sourceCodeParser,
            IEnumerable<INodeVisitor> nodeVisitors,
            IErrorRepository errorRepository,
            IGenerator generator,
            IConsole console)
        {
            _fileSystem = fileSystem;
            _sourceCodeParser = sourceCodeParser;
            _nodeVisitors = nodeVisitors;
            _errorRepository = errorRepository;
            _generator = generator;
            _console = console;
        }

        public async Task Run(FileInfo? inputFile)
        {
            if (inputFile == null)
            {
                _console.Error("Please specify a model file.");
                return;
            }

            if (!_fileSystem.File.Exists(inputFile.FullName))
            {
                _console.Error($"Could not find the model file '{inputFile.FullName}'.");
                return;
            }

            var fileStream = _fileSystem.File.OpenText(inputFile.FullName);
            var sourceCode = await fileStream.ReadToEndAsync();

            try
            {
                var sourceCodeDocument = _sourceCodeParser.Parse(sourceCode);
                await RunVisitors(sourceCodeDocument);
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

            await _generator.Generate(sourceCodeNode);
        }
    }
}