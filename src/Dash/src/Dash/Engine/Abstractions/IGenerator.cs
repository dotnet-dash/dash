using System.Threading.Tasks;
using Dash.Nodes;

namespace Dash.Engine.Abstractions
{
    public interface IGenerator
    {
        Task Generate(SourceCodeNode model);
    }
}
