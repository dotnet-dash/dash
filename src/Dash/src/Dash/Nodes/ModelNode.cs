using System.Collections.Generic;

namespace Dash.Nodes
{
    public class ModelNode : AstNode
    {
        public IList<EntityDeclarationNode> EntityDeclarations { get; } = new List<EntityDeclarationNode>();

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}