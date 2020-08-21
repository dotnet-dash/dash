using System.Linq;
using System.Threading.Tasks;
using Dash.Engine.Abstractions;
using Dash.Extensions;
using Dash.Nodes;

namespace Dash.Engine.Visitors
{
    public abstract class BaseVisitor : INodeVisitor
    {
        protected readonly IConsole Console;

        protected BaseVisitor(IConsole console)
        {
            Console = console;
        }

        public virtual async Task Visit(ModelNode node)
        {
            await node.EntityDeclarations.Accept(this);
        }

        public virtual async Task Visit(EntityDeclarationNode node)
        {
            Console.Trace($"{GetType().Name} visiting attributes of {node.Name}");
            await node.AttributeDeclarations.Accept(this);
            await node.InheritanceDeclarationNodes.Accept(this);
            await node.Has.Accept(this);
            await node.HasMany.Accept(this);
            await node.HasAndBelongsToMany.Accept(this);

            var childNodesCount = node.ChildNodes.Count;
            Console.Trace($"{GetType().Name} visiting {childNodesCount} child node(s) of {node.Name}");
            await node.ChildNodes.Accept(this);
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

        public virtual async Task Visit(CsvSeedDeclarationNode node)
        {
            await node.UriNode.Accept(this);
        }

        public virtual Task Visit(UriNode node)
        {
            return Task.CompletedTask;
        }
    }
}
