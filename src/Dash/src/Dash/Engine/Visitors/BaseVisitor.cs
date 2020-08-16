using Dash.Engine.Abstractions;
using Dash.Extensions;
using Dash.Nodes;

namespace Dash.Engine.Visitors
{
    public abstract class BaseVisitor : INodeVisitor
    {
        public virtual void Visit(ModelNode node)
        {
            node.EntityDeclarations.Accept(this);
        }

        public virtual void Visit(EntityDeclarationNode node)
        {
            node.AttributeDeclarations.Accept(this);
            node.InheritanceDeclarationNodes.Accept(this);
            node.Has.Accept(this);
            node.HasMany.Accept(this);
            node.HasAndBelongsToMany.Accept(this);
        }

        public virtual void Visit(AttributeDeclarationNode node)
        {
        }

        public virtual void Visit(HasReferenceDeclarationNode node)
        {
        }

        public virtual void Visit(HasManyReferenceDeclarationNode node)
        {
        }

        public virtual void Visit(HasAndBelongsToManyDeclarationNode node)
        {
        }

        public virtual void Visit(InheritanceDeclarationNode node)
        {
        }
    }
}
