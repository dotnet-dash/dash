// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Dash.Application;
using Dash.Common;
using Dash.Engine.Models;
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
        private readonly IModelRepository _modelRepository;
        private readonly IUriResourceRepository _uriResourceRepository;
        private readonly DashOptions _options;

        public DefaultGenerator(
            IUriResourceRepository uriResourceRepository,
            IConsole console,
            IBuildOutputRepository buildOutputRepository,
            ITemplateTransformer templateTransformer,
            IOptions<DashOptions> options,
            IFileSystem fileSystem,
            IModelRepository modelRepository)
        {
            _uriResourceRepository = uriResourceRepository;
            _console = console;
            _buildOutputRepository = buildOutputRepository;
            _templateTransformer = templateTransformer;
            _options = options.Value;
            _fileSystem = fileSystem;
            _modelRepository = modelRepository;
        }

        public async Task Generate(SourceCodeNode model)
        {
            var entities = _modelRepository.EntityModels
                .Where(e => !e.IsAbstract)
                .ToList();

            foreach (var templateNode in model.ConfigurationNode.Templates)
            {
                var template = await _uriResourceRepository.GetContents(templateNode.TemplateUriNode!.Uri);
                var directory = _fileSystem.AbsolutePath(templateNode.OutputUriNode!.Uri, _options);

                async Task Generate(string outputFilename, IEnumerable<EntityModel> listOfEntities)
                {
                    var output = await _templateTransformer.Transform(template, listOfEntities);
                    output = AddHeader(model.ConfigurationNode.Header!, output);
                    SaveFile(directory, outputFilename.AppendFilenameSuffix(model.ConfigurationNode.AutogenSuffix), output);
                }

                if (templateNode.OneClassPerFile)
                {
                    foreach (var e in entities)
                    {
                        await Generate(e.Name, new []{ e });
                    }
                }
                else
                {
                    await Generate(templateNode.TemplateUriNode!.Uri.Host, entities);
                }
            }
        }

        private void SaveFile(string directory, string outputFilename, string output)
        {
            var path = Path.Combine(directory, $"{outputFilename}.cs").NormalizeSlashes();

            _console.Trace($"Generating output: {path}");
            _buildOutputRepository.Add(path, output);
        }

        private static string AddHeader(string header, string output)
        {
            if (!string.IsNullOrWhiteSpace(header))
            {
                return $"// {header}{Environment.NewLine}{Environment.NewLine}{output}";
            }

            return output;
        }
    }
}