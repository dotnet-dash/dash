using Dash.Engine.Abstractions;
using Dash.Nodes;

namespace Dash.Engine.Visitors
{
    public class CreateJoinedEntityVisitor : BaseVisitor
    {
        private readonly IConsole _console;

        public CreateJoinedEntityVisitor(IConsole console)
        {
            _console = console;
        }

        public override void Visit(HasAndBelongsToManyDeclarationNode node)
        {
            var joinedEntityName = node.Parent.Name + node.ReferencedEntity;
            _console.Trace($"Adding joined entity: {joinedEntityName}");

            var joinedEntity = node.Parent.Parent.AddEntityDeclarationNode(joinedEntityName);
            joinedEntity.AddHasDeclaration(node.Parent.Name, node.Parent.Name);
            joinedEntity.AddHasDeclaration(node.ReferencedEntity, node.ReferencedEntity);

            base.Visit(node);
        }
    }
}
