// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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
        private readonly IFileSystem _fileSystem;
        private readonly IModelRepository _modelRepository;
        private readonly IConsole _console;
        private readonly IUriResourceRepository _uriResourceRepository;
        private readonly DashOptions _options;

        public DefaultGenerator(
            IUriResourceRepository uriResourceRepository,
            IFileSystem fileSystem,
            IModelRepository modelRepository,
            IConsole console,
            IOptions<DashOptions> options)
        {
            _uriResourceRepository = uriResourceRepository;
            _fileSystem = fileSystem;
            _modelRepository = modelRepository;
            _console = console;
            _options = options.Value;
        }

        public async Task Generate(SourceCodeNode model)
        {
            foreach (var templateNode in model.ConfigurationNode.Templates)
            {
                var templateContent = await _uriResourceRepository.GetContents(templateNode.TemplateUriNode!.Uri);
                var options = new Morestachio.ParserOptions(templateContent);
                var template = Morestachio.Parser.ParseWithOptions(options);

                var output = await template.CreateAndStringifyAsync
                (
                    new
                    {
                        Entities = _modelRepository.EntityModels
                    }
                );

                string directory = templateNode.OutputUriNode!.Uri.ToPath(_options);

                if (!_fileSystem.Directory.Exists(directory))
                {
                    _console.Trace($"Directory {directory} does not exist. Creating...");
                    _fileSystem.Directory.CreateDirectory(directory);
                }

                var path = Path.Combine(directory, $"{templateNode.TemplateUriNode!.Uri.Host}.generated.cs").NormalizeSlashes();
                _console.Info($"Generating file {path}");

                await _fileSystem.File.WriteAllTextAsync(path, output);
            }
        }
    }
}