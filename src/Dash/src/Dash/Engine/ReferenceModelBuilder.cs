using Dash.Engine.Abstractions;
using Dash.Engine.Models;
using Dash.Extensions;
using Dash.Nodes;

namespace Dash.Engine
{
    public class ReferenceModelBuilder : IModelBuilder
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

        public void Visit(ModelNode node)
        {
            node.EntityDeclarations.Accept(this);
        }

        public void Visit(EntityDeclarationNode node)
        {
            node.Has.Accept(this);
            node.HasMany.Accept(this);
            node.HasAndBelongsToMany.Accept(this);
        }

        public void Visit(AttributeDeclarationNode node)
        {
        }

        public void Visit(HasReferenceDeclarationNode node)
        {
            var result = _entityReferenceValueParser.Parse(node.ReferencedEntity);

            var referencedEntityModel = new ReferencedEntityModel(node.Name, result.EntityName!, result.IsNullable);

            _modelRepository
                .Get(node.Parent.Name)
                .SingleReferences
                .Add(referencedEntityModel);
        }

        public void Visit(HasManyReferenceDeclarationNode node)
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

        public void Visit(HasAndBelongsToManyDeclarationNode node)
        {
            var joinedEntity = new JoinedEntityModel(node.Parent.Name, node.ReferencedEntity);
            joinedEntity.SingleReferences.Add(new ReferencedEntityModel(node.Parent.Name, node.Parent.Name, false));
            joinedEntity.SingleReferences.Add(new ReferencedEntityModel(node.ReferencedEntity, node.ReferencedEntity, false));
            _modelRepository.Add(joinedEntity);

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
    }
}
