using Dash.Engine.Abstractions;
using Dash.Nodes;

namespace Dash.Engine.Visitors
{
    public class CreateJoinedEntityVisitor : BaseVisitor
    {
        private readonly IConsole _console;
        private ModelNode _modelNode = new ModelNode();

        public CreateJoinedEntityVisitor(IConsole console)
        {
            _console = console;
        }

        public override void Visit(ModelNode node)
        {
            _modelNode = node;

            base.Visit(node);
        }

        public override void Visit(HasAndBelongsToManyDeclarationNode node)
        {
            var joinedEntityName = node.Parent.Name + node.ReferencedEntity;
            _console.WriteLine($"Adding joined entity: {joinedEntityName}");

            var joinedEntity = new EntityDeclarationNode(joinedEntityName);
            joinedEntity.AddHasDeclaration(node.Parent.Name, node.Parent.Name);
            joinedEntity.AddHasDeclaration(node.ReferencedEntity, node.ReferencedEntity);

            _modelNode.EntityDeclarations.Add(joinedEntity);

            base.Visit(node);
        }
    }
}
