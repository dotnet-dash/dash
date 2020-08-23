using System.Threading.Tasks;
using Dash.Nodes;

namespace Dash.Engine
{
    public interface IGenerator
    {
        Task Generate(SourceCodeNode model);
    }
}
