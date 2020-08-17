using Dash.Engine.Abstractions;

namespace Dash.Nodes
{
    public class HasManyReferenceDeclarationNode : ReferenceDeclarationNode
    {
        public HasManyReferenceDeclarationNode(EntityDeclarationNode parent, string name, string referencedEntity) :
            base(parent, name, referencedEntity)
        {
        }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}