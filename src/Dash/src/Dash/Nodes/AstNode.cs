using System.Threading.Tasks;
using Dash.Engine;

namespace Dash.Nodes
{
    public abstract class AstNode
    {
        public abstract Task Accept(INodeVisitor visitor);
    }
}