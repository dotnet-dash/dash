using System.Collections.Generic;
using Dash.Nodes;

namespace Dash.Engine.Abstractions
{
    public interface ISemanticAnalyzer : INodeVisitor
    {
        IEnumerable<string> Errors { get; }
    }
}
