using System.Collections.Generic;
using Dash.Nodes;

namespace Dash.Engine.Abstractions
{
    public interface ISemanticAnalyzer
    {
        IEnumerable<string> Analyze(Model model);
    }
}
