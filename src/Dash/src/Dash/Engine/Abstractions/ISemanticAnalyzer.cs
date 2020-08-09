using System.Collections.Generic;
using Dash.Nodes;

namespace Dash.Engine.Abstractions
{
    interface ISemanticAnalyzer
    {
        IEnumerable<string> Analyze(Model model);
    }
}
