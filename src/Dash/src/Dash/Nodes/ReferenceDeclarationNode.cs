namespace Dash.Nodes
{
    public abstract class ReferenceDeclarationNode : AstNode
    {
        protected ReferenceDeclarationNode(EntityDeclarationNode parent, string name, string referencedEntity)
        {
            Parent = parent;
            Name = name;
            ReferencedEntity = referencedEntity;
        }

        public EntityDeclarationNode Parent { get; }

        public string Name { get; }

        public string ReferencedEntity { get; }
    }
}