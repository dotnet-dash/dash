// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.IO;
using System.Threading.Tasks;
using Dash.Application;
using Dash.Common;
using Dash.Extensions;
using Dash.Nodes;
using Microsoft.Extensions.Options;

namespace Dash.Engine.Generator
{
    public class DefaultGenerator : IGenerator
    {
        private readonly IConsole _console;
        private readonly IBuildOutputRepository _buildOutputRepository;
        private readonly ITemplateTransformer _templateTransformer;
        private readonly IUriResourceRepository _uriResourceRepository;
        private readonly DashOptions _options;

        public DefaultGenerator(
            IUriResourceRepository uriResourceRepository,
            IConsole console,
            IBuildOutputRepository buildOutputRepository,
            ITemplateTransformer templateTransformer,
            IOptions<DashOptions> options)
        {
            _uriResourceRepository = uriResourceRepository;
            _console = console;
            _buildOutputRepository = buildOutputRepository;
            _templateTransformer = templateTransformer;
            _options = options.Value;
        }

        public async Task Generate(SourceCodeNode model)
        {
            foreach (var templateNode in model.ConfigurationNode.Templates)
            {
                var template = await _uriResourceRepository.GetContents(templateNode.TemplateUriNode!.Uri);
                var output = await _templateTransformer.Transform(template);

                var directory = templateNode.OutputUriNode!.Uri.ToPath(_options);
                var path = Path
                    .Combine(directory, $"{templateNode.TemplateUriNode!.Uri.Host}.generated.cs")
                    .NormalizeSlashes();

                _console.Trace($"Generating output: {path}");
                _buildOutputRepository.Add(path, output);
            }
        }
    }
}