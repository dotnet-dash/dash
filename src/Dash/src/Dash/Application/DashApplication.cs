using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Dash.Engine.Abstractions;

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

        public async Task Run(FileInfo inputFile)
        {
            var fileStream = _fileSystem.File.OpenText(inputFile.FullName);
            var sourceCode = await fileStream.ReadToEndAsync();

            var sourceCodeDocument = _sourceCodeParser.Parse(sourceCode);

            foreach (var visitor in _nodeVisitors)
            {
                _console.Trace($"Running {visitor.GetType()}");
                visitor.Visit(sourceCodeDocument.ModelNode);

                if (_errorRepository.HasErrors())
                {
                    var errors = string.Join(Environment.NewLine, _errorRepository.GetErrors().Select(e => e));
                    _console.Error(errors);

                    return;
                }
            }

            await _generator.Generate(sourceCodeDocument);
        }
    }
}