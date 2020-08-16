using System.Collections.Generic;
using System.Linq;
using Dash.Engine.Abstractions;
using Dash.Engine.Models;
using Dash.Extensions;
using Dash.Nodes;

namespace Dash.Engine.Visitors
{
    public class DefaultModelBuilder : BaseVisitor, IModelBuilder
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

        public override void Visit(EntityDeclarationNode node)
        {
            var model = new EntityModel(node.Name);
            _modelRepository.Add(model);

            base.Visit(node);
        }

        public override void Visit(AttributeDeclarationNode node)
        {
            var result = _dataTypeParser.Parse(node.AttributeDataType);

            var codeDataType = _codeLanguageProvider.Translate(result.DataType);
            var databaseDataType = _databaseLanguageProvider.Translate(result.DataType);

            var entityModel = _modelRepository.Get(node.Parent.Name);
            entityModel.CodeAttributes.Add(new AttributeModel(node.AttributeName, codeDataType, result.IsNullable));
            entityModel.DataAttributes.Add(new AttributeModel(node.AttributeName, databaseDataType, result.IsNullable));

            base.Visit(node);
        }
    }
}
