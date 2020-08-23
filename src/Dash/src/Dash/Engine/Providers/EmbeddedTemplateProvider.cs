using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Dash.Exceptions;

namespace Dash.Engine.Providers
{
    public class EmbeddedTemplateProvider : IEmbeddedTemplateProvider
    {
        public Task<string> GetTemplate(string templateName)
        {
            if (!TryGetStream(templateName, out var stream))
            {
                throw new EmbeddedTemplateNotFoundException(templateName);
            }

            TextReader tr = new StreamReader(stream!);
            return tr.ReadToEndAsync();
        }

        private static bool TryGetStream(string templateName, out Stream? stream)
        {
            stream = Assembly
                    .GetAssembly(typeof(EmbeddedTemplateProvider))!
                    .GetManifestResourceStream("Dash.Templates." + templateName.ToLower());

            return stream != null;
        }
    }
}