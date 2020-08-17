using System.Threading.Tasks;

namespace Dash.Engine.Abstractions
{
    public interface ITemplateProvider
    {
        Task<string> GetTemplate(string templateName);
    }
}