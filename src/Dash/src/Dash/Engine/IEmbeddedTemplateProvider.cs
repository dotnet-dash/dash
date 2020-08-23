using System.Threading.Tasks;

namespace Dash.Engine
{
    public interface IEmbeddedTemplateProvider
    {
        Task<string> GetTemplate(string templateName);
    }
}