using System.Collections.Generic;

namespace Dash.Engine.Abstractions
{
    public interface ISemanticAnalyzer : INodeVisitor
    {
        IEnumerable<string> Errors { get; }
    }
}
