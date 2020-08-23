using Dash.Nodes;

namespace Dash.Engine
{
    public interface ISourceCodeParser
    {
        SourceCodeNode Parse(string sourceCode);
    }
}
