using System.Linq;
using System.Threading.Tasks;
using Dash.Common.Abstractions;
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

        public SetInheritanceVisitor(IConsole console) : base(console)
        {
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
                Console.Trace($"No custom inheritance defined for '{node.Name}', setting inheritance to '{BaseEntityName}'");
                node.AddInheritanceDeclaration(BaseEntityName);
            }

            return base.Visit(node);
        }

        private EntityDeclarationNode GetOrAddBaseEntityIfNotDeclared(ModelNode node)
        {
            var baseEntity = node.EntityDeclarations.SingleOrDefault(e => e.Name.IsSame(BaseEntityName));
            if (baseEntity == null)
            {
                Console.Trace($"No entity '{BaseEntityName}' declared. Adding '{BaseEntityName}'");
                baseEntity = node.AddEntityDeclarationNode(BaseEntityName);
            }

            return baseEntity;
        }

        private void AddIdAttributeIfNotDeclared(EntityDeclarationNode baseEntity)
        {
            var idAttribute = baseEntity.AttributeDeclarations.FirstOrDefault(e => e.AttributeName.IsSame(BaseEntityIdAttributeName));
            if (idAttribute == null)
            {
                Console.Trace($"No attribute '{BaseEntityIdAttributeName}' declared. Adding '{BaseEntityIdAttributeName}'");
                baseEntity.InsertAttributeDeclaration(0, BaseEntityIdAttributeName, BaseEntityIdAttributeDataType);
            }
        }
    }
}
