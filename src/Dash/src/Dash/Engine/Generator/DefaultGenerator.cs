﻿// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.IO;
using System.IO.Abstractions;
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
        private readonly IFileSystem _fileSystem;
        private readonly IUriResourceRepository _uriResourceRepository;
        private readonly DashOptions _options;

        public DefaultGenerator(
            IUriResourceRepository uriResourceRepository,
            IConsole console,
            IBuildOutputRepository buildOutputRepository,
            ITemplateTransformer templateTransformer,
            IOptions<DashOptions> options,
            IFileSystem fileSystem)
        {
            _uriResourceRepository = uriResourceRepository;
            _console = console;
            _buildOutputRepository = buildOutputRepository;
            _templateTransformer = templateTransformer;
            _fileSystem = fileSystem;
            _options = options.Value;
        }

        public async Task Generate(SourceCodeNode model)
        {
            foreach (var templateNode in model.ConfigurationNode.Templates)
            {
                var template = await _uriResourceRepository.GetContents(templateNode.TemplateUriNode!.Uri);
                var output = await _templateTransformer.Transform(template);

                if (!string.IsNullOrWhiteSpace(model.ConfigurationNode.Header))
                {
                    output = "// " +
                             model.ConfigurationNode.Header +
                             Environment.NewLine +
                             Environment.NewLine +
                             output;
                }

                var directory = _fileSystem.AbsolutePath(templateNode.OutputUriNode!.Uri, _options);

                var suffix = model.ConfigurationNode.AutogenSuffix;
                if (!string.IsNullOrWhiteSpace(suffix))
                {
                    suffix = "." + suffix.Trim('.');
                }

                var path = Path
                    .Combine(directory, $"{templateNode.TemplateUriNode!.Uri.Host}{suffix}.cs")
                    .NormalizeSlashes();

                _console.Trace($"Generating output: {path}");
                _buildOutputRepository.Add(path, output);
            }
        }
    }
}