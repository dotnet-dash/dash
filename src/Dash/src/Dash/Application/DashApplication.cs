using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dash.Engine.Abstractions;
using Microsoft.Extensions.Options;

namespace Dash.Application
{
    public class DashApplication
    {
        private readonly IParser _parser;
        private readonly ISemanticAnalyzer _semanticAnalyzer;
        private readonly IGenerator _generator;
        private readonly IOptions<DashOptions> _dashOptions;

        public DashApplication(IParser parser, ISemanticAnalyzer semanticAnalyzer, IGenerator generator, IOptions<DashOptions> dashOptions)
        {
            _parser = parser;
            _semanticAnalyzer = semanticAnalyzer;
            _generator = generator;
            _dashOptions = dashOptions;
        }

        public async Task Run(FileInfo inputFile)
        {
            var fileStream = inputFile.OpenText();
            var sourceCode = await fileStream.ReadToEndAsync();

            var model = _parser.Parse(sourceCode);
            if (model.Errors.Any())
            {
                foreach (var error in model.Errors)
                {
                    Console.Error.WriteLine(error);
                }

                return;
            }

            var semanticErrors = _semanticAnalyzer.Analyze(model);
            if (semanticErrors.Any())
            {
                foreach (var error in semanticErrors)
                {
                    Console.Error.WriteLine(error);
                }

                return;
            }

            await _generator.Generate(model);
        }
    }
}