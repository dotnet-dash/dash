using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Dash.Engine.Abstractions;
using Dash.Engine.Models.SourceCode;

namespace Dash.Engine.Generator
{
    public class DefaultGenerator : IGenerator
    {
        private readonly ITemplateProvider _templateProvider;
        private readonly IFileSystem _fileSystem;
        private readonly IModelRepository _modelRepository;
        private readonly IConsole _console;

        public DefaultGenerator(
            ITemplateProvider templateProvider,
            IFileSystem fileSystem,
            IModelRepository modelRepository,
            IConsole console)
        {
            _templateProvider = templateProvider;
            _fileSystem = fileSystem;
            _modelRepository = modelRepository;
            _console = console;
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
                _console.Info($"Generating file {path}");

                await _fileSystem.File.WriteAllTextAsync(path, output);
            }
        }
    }
}