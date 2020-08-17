using Dash.Engine.Abstractions;

namespace Dash.Nodes
{
    public class HasAndBelongsToManyDeclarationNode : ReferenceDeclarationNode
    {
        public HasAndBelongsToManyDeclarationNode(EntityDeclarationNode parent, string name, string referencedEntity) :
            base(parent, name, referencedEntity)
        {
        }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}