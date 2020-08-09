using Dash.Nodes;

namespace Dash.Engine.Abstractions
{
    public interface IParser
    {
        Model Parse(string sourceCode);
    }
}
