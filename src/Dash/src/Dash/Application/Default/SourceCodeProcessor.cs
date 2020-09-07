// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dash.Common;
using Dash.Engine;
using Dash.Nodes;

namespace Dash.Application.Default
{
    public class SourceCodeProcessor : ISourceCodeProcessor
    {
        private readonly IConsole _console;
        private readonly IEnumerable<INodeVisitor> _nodeVisitors;
        private readonly IErrorRepository _errorRepository;
        private readonly IGenerator _generator;
        private readonly IEnumerable<IPostGenerator> _postGenerators;

        public SourceCodeProcessor(
            IConsole console,
            IEnumerable<INodeVisitor> nodeVisitors,
            IErrorRepository errorRepository,
            IGenerator generator,
            IEnumerable<IPostGenerator> postGenerators)
        {
            _console = console;
            _nodeVisitors = nodeVisitors;
            _errorRepository = errorRepository;
            _generator = generator;
            _postGenerators = postGenerators;
        }

        public async Task WalkTree(SourceCodeNode sourceCodeNode)
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
