using System.Threading.Tasks;
using Dash.Common.Abstractions;
using Dash.Engine.Abstractions;
using Dash.Nodes;

namespace Dash.Engine.Visitors
{
    public class CreateJoinedEntityVisitor : BaseVisitor
    {
        public CreateJoinedEntityVisitor(IConsole console) : base(console)
        {
        }

        public override Task Visit(HasAndBelongsToManyDeclarationNode node)
        {
            var joinedEntityName = node.Parent.Name + node.ReferencedEntity;
            Console.Trace($"Adding joined entity: {joinedEntityName}");

            var joinedEntity = node.Parent.Parent.AddEntityDeclarationNode(joinedEntityName);
            joinedEntity.AddHasDeclaration(node.Parent.Name, node.Parent.Name);
            joinedEntity.AddHasDeclaration(node.ReferencedEntity, node.ReferencedEntity);

            return base.Visit(node);
        }
    }
}
