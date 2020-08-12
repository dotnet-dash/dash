using System.IO.Abstractions;
using System.Text;
using System.Threading.Tasks;
using Dash.Application;
using Dash.Engine.Abstractions;
using Dash.Nodes;
using Microsoft.Extensions.Options;
using Morestachio.Formatter.Framework;

namespace Dash.Engine.Generator
{
    public class DefaultGenerator : IGenerator
    {
        private readonly ITemplateProvider _templateProvider;
        private readonly IFileSystem _fileSystem;
        private readonly IOptions<DashOptions> _dashOptions;

        public DefaultGenerator(ITemplateProvider templateProvider, IFileSystem fileSystem, IOptions<DashOptions> dashOptions)
        {
            _templateProvider = templateProvider;
            _fileSystem = fileSystem;
            _dashOptions = dashOptions;
        }

        public async Task Generate(Model model)
        {
            foreach (var templateName in _dashOptions.Value.Templates)
            {
                var templateContent = await _templateProvider.GetTemplate(templateName);
                var options = new Morestachio.ParserOptions(templateContent);
                var template = Morestachio.Parser.ParseWithOptions(options);

                var output = await template.CreateAndStringifyAsync(model);
                await _fileSystem.File.WriteAllTextAsync($"./{templateName}.generated.cs", output, Encoding.UTF8);
            }
        }
    }
}