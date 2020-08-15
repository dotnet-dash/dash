using Dash.Engine.Abstractions;

namespace Dash.Nodes
{
    public abstract class AstNode
    {
        public abstract void Accept(INodeVisitor visitor);
    }
}