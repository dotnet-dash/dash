using System.Threading.Tasks;
using Dash.Engine.Abstractions;
using Dash.Extensions;
using Dash.Nodes;

namespace Dash.Engine.Visitors
{
    public abstract class BaseVisitor : INodeVisitor
    {
        public virtual Task Visit(ModelNode node)
        {
            node.EntityDeclarations.Accept(this);
            return Task.CompletedTask;
        }

        public virtual Task Visit(EntityDeclarationNode node)
        {
            node.AttributeDeclarations.Accept(this);
            node.InheritanceDeclarationNodes.Accept(this);
            node.Has.Accept(this);
            node.HasMany.Accept(this);
            node.HasAndBelongsToMany.Accept(this);
            return Task.CompletedTask;
        }

        public virtual Task Visit(AttributeDeclarationNode node)
        {
            return Task.CompletedTask;
        }

        public virtual Task Visit(HasReferenceDeclarationNode node)
        {
            return Task.CompletedTask;
        }

        public virtual Task Visit(HasManyReferenceDeclarationNode node)
        {
            return Task.CompletedTask;
        }

        public virtual Task Visit(HasAndBelongsToManyDeclarationNode node)
        {
            return Task.CompletedTask;
        }

        public virtual Task Visit(InheritanceDeclarationNode node)
        {
            return Task.CompletedTask;
        }

        public virtual Task Visit(CsvSeedDeclarationNode node)
        {
            return Task.CompletedTask;
        }

        public virtual Task Visit(UriNode node)
        {
            return Task.CompletedTask;
        }
    }
}
