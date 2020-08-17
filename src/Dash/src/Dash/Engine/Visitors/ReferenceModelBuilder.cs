using Dash.Engine.Abstractions;
using Dash.Engine.Models;
using Dash.Nodes;

namespace Dash.Engine.Visitors
{
    public class ReferenceModelBuilder : BaseVisitor, IModelBuilder
    {
        private readonly IModelRepository _modelRepository;
        private readonly IEntityReferenceValueParser _entityReferenceValueParser;

        public ReferenceModelBuilder(
            IModelRepository modelRepository,
            IEntityReferenceValueParser entityReferenceValueParser)
        {
            _modelRepository = modelRepository;
            _entityReferenceValueParser = entityReferenceValueParser;
        }

        public override void Visit(HasReferenceDeclarationNode node)
        {
            var result = _entityReferenceValueParser.Parse(node.ReferencedEntity);

            var referencedEntityModel = new ReferencedEntityModel(node.Name, result.EntityName!, result.IsNullable);

            _modelRepository
                .Get(node.Parent.Name)
                .SingleReferences
                .Add(referencedEntityModel);
        }

        public override void Visit(HasManyReferenceDeclarationNode node)
        {
            var result = _entityReferenceValueParser.Parse(node.ReferencedEntity);

            var referencedEntityModel = new ReferencedEntityModel(node.Name, result.EntityName!, result.IsNullable);
            _modelRepository
                .Get(node.Parent.Name)
                .CollectionReferences
                .Add(referencedEntityModel);

            var singleReference = new ReferencedEntityModel(node.Parent.Name, node.Parent.Name, false);
            _modelRepository
                .Get(node.ReferencedEntity)
                .SingleReferences
                .Add(singleReference);
        }

        public override void Visit(HasAndBelongsToManyDeclarationNode node)
        {
            var joinedEntity = new JoinedEntityModel(node.Parent.Name, node.ReferencedEntity);

            var referencedEntityModel = new ReferencedEntityModel(joinedEntity.Name, joinedEntity.Name, false);

            _modelRepository
                .Get(node.Parent.Name)
                .CollectionReferences
                .Add(referencedEntityModel);

            _modelRepository
                .Get(node.ReferencedEntity)
                .CollectionReferences
                .Add(referencedEntityModel);
        }

        public override void Visit(InheritanceDeclarationNode node)
        {
            _modelRepository
                .Get(node.Parent.Name)
                .InheritAttributes(_modelRepository.Get(node.InheritedEntity));
        }
    }
}
