using System.Threading.Tasks;
using Dash.Engine;

namespace Dash.Nodes
{
    public class AbstractDeclarationNode : AstNode
    {
        public AbstractDeclarationNode(EntityDeclarationNode parent, bool value)
        {
            Parent = parent;
            Value = value;
        }

        public EntityDeclarationNode Parent { get; }

        public bool Value { get; }

        public override async Task Accept(INodeVisitor visitor)
        {
            await visitor.Visit(this);
        }
    }
}
