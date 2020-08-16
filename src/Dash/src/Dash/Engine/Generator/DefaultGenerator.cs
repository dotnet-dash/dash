﻿using System;
using System.IO;
using System.IO.Abstractions;
using System.Text;
using System.Threading.Tasks;
using Dash.Application;
using Dash.Engine.Abstractions;
using Dash.Engine.Models.SourceCode;
using Microsoft.Extensions.Options;

namespace Dash.Engine.Generator
{
    public class DefaultGenerator : IGenerator
    {
        private readonly ITemplateProvider _templateProvider;
        private readonly IFileSystem _fileSystem;
        private readonly IModelRepository _modelRepository;
        private readonly DashOptions _dashOptions;

        public DefaultGenerator(
            ITemplateProvider templateProvider,
            IFileSystem fileSystem,
            IModelRepository modelRepository,
            IOptions<DashOptions> dashOptions)
        {
            _templateProvider = templateProvider;
            _fileSystem = fileSystem;
            _modelRepository = modelRepository;
            _dashOptions = dashOptions.Value;
        }

        public async Task Generate(SourceCodeDocument model)
        {
            foreach (var templateNode in model.Configuration.Templates)
            {
                var templateContent = await _templateProvider.GetTemplate(templateNode.Template!);
                var options = new Morestachio.ParserOptions(templateContent);
                var template = Morestachio.Parser.ParseWithOptions(options);

                var output = await template.CreateAndStringifyAsync
                (
                    new
                    {
                        Entities = _modelRepository.EntityModels
                    }
                );

                var path = Path.Combine(templateNode.Output, $"{templateNode.Template!}.generated.cs");
                Console.Out.WriteLine($"Writing to file {path}");

                await _fileSystem.File.WriteAllTextAsync(path, output, Encoding.UTF8);
            }
        }
    }
}