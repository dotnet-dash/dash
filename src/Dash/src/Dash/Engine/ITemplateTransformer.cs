using System.Threading.Tasks;

namespace Dash.Engine
{
    public interface ITemplateTransformer
    {
        Task<string> Transform(string template);
    }
}
