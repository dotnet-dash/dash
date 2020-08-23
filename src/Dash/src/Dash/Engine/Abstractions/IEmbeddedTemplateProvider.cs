using System.Threading.Tasks;

namespace Dash.Engine.Abstractions
{
    public interface IEmbeddedTemplateProvider
    {
        Task<string> GetTemplate(string templateName);
    }
}