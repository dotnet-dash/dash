using Dash.Engine.Models.SourceCode;

namespace Dash.Engine.Abstractions
{
    public interface ISourceCodeParser
    {
        SourceCodeDocument Parse(string sourceCode);
    }
}
