using Dash.Nodes;

namespace Dash.Engine.Abstractions
{
    public interface IParser
    {
        Model Parse(string sourceCode);
    }

    public interface IParser2
    {
        ModelNode Parse(string sourceCode);
    }
}
