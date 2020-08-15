using Dash.Nodes;

namespace Dash.Engine.Abstractions
{
    public interface INodeVisitor
    {
        void Visit(ModelNode node);
        void Visit(EntityDeclarationNode node);
        void Visit(AttributeDeclarationNode node);
        void Visit(ReferenceDeclarationNode node);
    }
}