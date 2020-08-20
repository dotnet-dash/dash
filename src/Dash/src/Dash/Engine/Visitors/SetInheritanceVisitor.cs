using System.Linq;
using System.Threading.Tasks;
using Dash.Engine.Abstractions;
using Dash.Extensions;
using Dash.Nodes;

namespace Dash.Engine.Visitors
{
    public class SetInheritanceVisitor : BaseVisitor
    {
        private const string BaseEntityName = "Base";
        private const string BaseEntityIdAttributeName = "Id";
        private const string BaseEntityIdAttributeDataType = "Int";
        private readonly IConsole _console;

        public SetInheritanceVisitor(IConsole console)
        {
            _console = console;
        }

        public override Task Visit(ModelNode node)
        {
            var entity = GetOrAddBaseEntityIfNotDeclared(node);
            AddIdAttributeIfNotDeclared(entity);

            return base.Visit(node);
        }

        public override Task Visit(EntityDeclarationNode node)
        {
            if (!node.InheritanceDeclarationNodes.Any() && !node.Name.IsSame(BaseEntityName))
            {
                _console.Trace($"No custom inheritance defined for '{node.Name}', setting inheritance to '{BaseEntityName}'");
                node.AddInheritanceDeclaration(BaseEntityName);
            }

            return base.Visit(node);
        }

        private EntityDeclarationNode GetOrAddBaseEntityIfNotDeclared(ModelNode node)
        {
            var baseEntity = node.EntityDeclarations.SingleOrDefault(e => e.Name.IsSame(BaseEntityName));
            if (baseEntity == null)
            {
                _console.Trace($"No entity '{BaseEntityName}' declared. Adding '{BaseEntityName}'");
                baseEntity = node.AddEntityDeclarationNode(BaseEntityName);
            }

            return baseEntity;
        }

        private void AddIdAttributeIfNotDeclared(EntityDeclarationNode baseEntity)
        {
            var idAttribute = baseEntity.AttributeDeclarations.FirstOrDefault(e => e.AttributeName.IsSame(BaseEntityIdAttributeName));
            if (idAttribute == null)
            {
                _console.Trace($"No attribute '{BaseEntityIdAttributeName}' declared. Adding '{BaseEntityIdAttributeName}'");
                baseEntity.InsertAttributeDeclaration(0, BaseEntityIdAttributeName, BaseEntityIdAttributeDataType);
            }
        }
    }
}
