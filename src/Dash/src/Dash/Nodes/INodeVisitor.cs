namespace Dash.Nodes
{
    public interface INodeVisitor
    {
        void Visit(ModelNode node);
        void Visit(EntityDeclarationNode expression);
        void Visit(AttributeDeclarationNode expression);
        void Visit(ReferenceDeclarationNode declarationNode);
    }
}