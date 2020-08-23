using System.Threading.Tasks;
using Dash.Engine;

namespace Dash.Nodes
{
    public class InheritanceDeclarationNode : AstNode
    {
        public InheritanceDeclarationNode(EntityDeclarationNode parent, string inheritedEntity)
        {
            Parent = parent;
            InheritedEntity = inheritedEntity;
        }

        public EntityDeclarationNode Parent { get; set; }

        public string InheritedEntity { get; set; }

        public override async Task Accept(INodeVisitor visitor)
        {
            await visitor.Visit(this);
        }
    }
}
