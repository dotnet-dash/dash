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
        private readonly IModelRepository _modelRepository;
        private readonly ILanguageProvider _codeLanguageProvider;
        private readonly ILanguageProvider _databaseLanguageProvider;

        public DefaultModelBuilder(
            IDataTypeParser dataTypeParser,
            IEnumerable<ILanguageProvider> languageProviders,
            IModelRepository modelRepository)
        {
            _dataTypeParser = dataTypeParser;
            _modelRepository = modelRepository;
            _codeLanguageProvider = languageProviders.Single(e => e.Name.IsSame("cs"));
            _databaseLanguageProvider = languageProviders.Single(e => e.Name.IsSame("SqlServer"));
        }

        public void Visit(ModelNode node)
        {
            node.EntityDeclarations.Accept(this);
        }

        public void Visit(EntityDeclarationNode node)
        {
            var model = new EntityModel(node.Name);
            _modelRepository.Add(model);

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

            var entityModel = _modelRepository.Get(node.Parent.Name);
            entityModel.CodeAttributes.Add(new AttributeModel(node.AttributeName, codeDataType, result.IsNullable));
            entityModel.DataAttributes.Add(new AttributeModel(node.AttributeName, databaseDataType, result.IsNullable));
        }

        public void Visit(HasReferenceDeclarationNode node)
        {
        }

        public void Visit(HasManyReferenceDeclarationNode node)
        {
        }

        public void Visit(HasAndBelongsToManyDeclarationNode node)
        {
        }
    }
}
