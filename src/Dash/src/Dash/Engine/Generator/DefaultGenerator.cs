using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Dash.Common.Abstractions;
using Dash.Engine.Abstractions;
using Dash.Nodes;

namespace Dash.Engine.Generator
{
    public class DefaultGenerator : IGenerator
    {
        private readonly IFileSystem _fileSystem;
        private readonly IModelRepository _modelRepository;
        private readonly IConsole _console;
        private readonly IUriResourceRepository _uriResourceRepository;

        public DefaultGenerator(
            IUriResourceRepository uriResourceRepository,
            IFileSystem fileSystem,
            IModelRepository modelRepository,
            IConsole console)
        {
            _uriResourceRepository = uriResourceRepository;
            _fileSystem = fileSystem;
            _modelRepository = modelRepository;
            _console = console;
        }

        public async Task Generate(SourceCodeNode model)
        {
            foreach (var templateNode in model.ConfigurationNode.Templates)
            {
                var templateContent = await _uriResourceRepository.GetContents(templateNode.Template!);
                var options = new Morestachio.ParserOptions(templateContent);
                var template = Morestachio.Parser.ParseWithOptions(options);

                var output = await template.CreateAndStringifyAsync
                (
                    new
                    {
                        Entities = _modelRepository.EntityModels
                    }
                );

                var absolutePath = templateNode.OutputUriNode!.Uri.AbsolutePath;
                if (!_fileSystem.Directory.Exists(absolutePath))
                {
                    _console.Trace($"Directory {absolutePath} does not exist. Creating....");
                    _fileSystem.Directory.CreateDirectory(absolutePath);
                }

                var path = Path.Combine(absolutePath, $"{templateNode.Template!.Host}.generated.cs");
                _console.Info($"Generating file {path}");

                await _fileSystem.File.WriteAllTextAsync(path, output);
            }
        }
    }
}