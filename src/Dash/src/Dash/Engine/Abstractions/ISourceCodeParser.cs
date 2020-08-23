using Dash.Nodes;

namespace Dash.Engine.Abstractions
{
    public interface ISourceCodeParser
    {
        SourceCodeNode Parse(string sourceCode);
    }
}
