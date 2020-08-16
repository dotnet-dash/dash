using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dash.Engine.Abstractions;
using Dash.Extensions;
using Dash.Nodes;

namespace Dash.Application
{
    public class DashApplication
    {
        private readonly ISourceCodeParser _sourceCodeParser;
        private readonly ISymbolCollector _symbolCollector;
        private readonly ISemanticAnalyzer _semanticAnalyzer;
        private readonly IEnumerable<IModelBuilder> _modelBuilders;
        private readonly IGenerator _generator;

        public DashApplication(ISourceCodeParser sourceCodeParser,
            ISymbolCollector symbolCollector,
            ISemanticAnalyzer semanticAnalyzer,
            IEnumerable<IModelBuilder> modelBuilders,
            IGenerator generator)
        {
            _sourceCodeParser = sourceCodeParser;
            _symbolCollector = symbolCollector;
            _semanticAnalyzer = semanticAnalyzer;
            _modelBuilders = modelBuilders;
            _generator = generator;
        }

        public async Task Run(FileInfo inputFile)
        {
            var fileStream = inputFile.OpenText();
            var sourceCode = await fileStream.ReadToEndAsync();

            var sourceCodeDocument = _sourceCodeParser.Parse(sourceCode);

            _symbolCollector.Visit(sourceCodeDocument.ModelNode);
            if (!SemanticAnalyzer(sourceCodeDocument.ModelNode))
            {
                return;
            }

            _modelBuilders.Visit(sourceCodeDocument.ModelNode);

            await _generator.Generate(sourceCodeDocument);
        }

        private bool SemanticAnalyzer(ModelNode modelNode)
        {
            _semanticAnalyzer.Visit(modelNode);

            foreach (var error in _semanticAnalyzer.Errors)
            {
                Console.Error.WriteLine(error);
            }

            return !_semanticAnalyzer.Errors.Any();
        }
    }
}