using System;
using System.Collections.Generic;
using System.Linq;
using Dash.Engine.Abstractions;
using Dash.Engine.Models;
using Dash.Extensions;
using Dash.Nodes;

namespace Dash.Engine
{
    public class DefaultModelBuilder : IModelBuilder
    {
        private readonly IDataTypeParser _dataTypeParser;
        private readonly IEntityReferenceValueParser _entityReferenceValueParser;
        private readonly IDictionary<string, EntityModel> _entityModels = new Dictionary<string, EntityModel>(StringComparer.OrdinalIgnoreCase);
        private readonly ILanguageProvider _codeLanguageProvider;
        private readonly ILanguageProvider _databaseLanguageProvider;

        public IEnumerable<EntityModel> EntityModels => _entityModels.Select(e => e.Value).ToList();

        public DefaultModelBuilder(
            IDataTypeParser dataTypeParser,
            IEntityReferenceValueParser entityReferenceValueParser,
            IEnumerable<ILanguageProvider> languageProviders)
        {
            _dataTypeParser = dataTypeParser;
            _entityReferenceValueParser = entityReferenceValueParser;
            _codeLanguageProvider = languageProviders.Single(e => e.Name.IsSame("cs"));
            _databaseLanguageProvider = languageProviders.Single(e => e.Name.IsSame("SqlServer"));
        }

        public void Visit(ModelNode node)
        {
            foreach (var entityDeclarationNode in node.EntityDeclarations)
            {
                entityDeclarationNode.Accept(this);
            }
        }

        public void Visit(EntityDeclarationNode node)
        {
            var model = new EntityModel(node.Name);
            _entityModels.Add(node.Name, model);

            foreach (var attribute in node.AttributeDeclarations)
            {
                attribute.Accept(this);
            }

            foreach (var referenceNode in node.Has)
            {
                referenceNode.Accept(this);
            }

            foreach (var referenceNode in node.HasMany)
            {
                referenceNode.Accept(this);
            }

            foreach (var referenceNode in node.HasAndBelongsToMany)
            {
                referenceNode.Accept(this);
            }
        }

        public void Visit(AttributeDeclarationNode node)
        {
            var result = _dataTypeParser.Parse(node.AttributeDataType);

            var codeDataType = _codeLanguageProvider.Translate(result.DataType);
            var databaseDataType = _databaseLanguageProvider.Translate(result.DataType);

            if (_entityModels.TryGetValue(node.Parent.Name, out var entityModel))
            {
                entityModel.CodeAttributeModels.Add(new AttributeModel(node.AttributeName, codeDataType, result.IsNullable));
                entityModel.DataAttributeModels.Add(new AttributeModel(node.AttributeName, databaseDataType, result.IsNullable));
            }
        }

        public void Visit(HasReferenceDeclarationNode node)
        {
            var result = _entityReferenceValueParser.Parse(node.ReferencedEntity);

            var referencedEntityModel = new ReferencedEntityModel(node.Name, result.EntityName!, result.IsNullable);

            _entityModels[node.Parent.Name].SingleReferences.Add(referencedEntityModel);
        }

        public void Visit(HasManyReferenceDeclarationNode node)
        {
            var result = _entityReferenceValueParser.Parse(node.ReferencedEntity);

            var referencedEntityModel = new ReferencedEntityModel(node.Name, result.EntityName!, result.IsNullable);
            _entityModels[node.Parent.Name].CollectionReferences.Add(referencedEntityModel);

            var singleReference = new ReferencedEntityModel(node.Parent.Name, node.Parent.Name, false);
            _entityModels[node.ReferencedEntity].SingleReferences.Add(singleReference);
        }

        public void Visit(HasAndBelongsToManyDeclarationNode node)
        {
            var joinedEntity = new JoinedEntityModel(node.Parent.Name, node.ReferencedEntity);
            joinedEntity.SingleReferences.Add(new ReferencedEntityModel(node.Parent.Name, node.Parent.Name, false));
            joinedEntity.SingleReferences.Add(new ReferencedEntityModel(node.ReferencedEntity, node.ReferencedEntity, false));
            _entityModels[joinedEntity.Name] = joinedEntity;

            var referencedEntityModel = new ReferencedEntityModel(joinedEntity.Name, joinedEntity.Name, false);
            _entityModels[node.Parent.Name].CollectionReferences.Add(referencedEntityModel);
            _entityModels[node.ReferencedEntity].CollectionReferences.Add(referencedEntityModel);
        }
    }
}
