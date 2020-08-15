using Dash.Nodes;

namespace Dash.Engine.Abstractions
{
    public interface INodeVisitor
    {
        void Visit(ModelNode node);
        void Visit(EntityDeclarationNode node);
        void Visit(AttributeDeclarationNode node);
        void Visit(HasReferenceDeclarationNode node);
        void Visit(HasManyReferenceDeclarationNode node);
        void Visit(HasAndBelongsToManyDeclarationNode node);
    }
}