using Dash.Engine.Models.SourceCode;

namespace Dash.Engine.Abstractions
{
    public interface ISourceCodeParser
    {
        SourceCode Parse(string sourceCode);
    }
}
