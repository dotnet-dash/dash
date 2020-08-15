namespace Dash.Nodes
{
    public class ReferenceDeclarationNode : AstNode
    {
        public ReferenceDeclarationNode(string name, string referencedEntity)
        {
            Name = name;
            ReferencedEntity = referencedEntity;
        }

        public string Name { get; }

        public string ReferencedEntity { get; }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}