// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dash.Common;
using Dash.Engine.DataTypes;
using Dash.Engine.Models;
using Dash.Extensions;
using Dash.Nodes;

namespace Dash.Engine.Visitors
{
    public class DefaultModelBuilder : BaseVisitor
    {
        private readonly IDataTypeParser _dataTypeParser;
        private readonly IModelRepository _modelRepository;
        private readonly ILanguageProvider _codeLanguageProvider;
        private readonly ILanguageProvider _databaseLanguageProvider;

        public DefaultModelBuilder(
            IDataTypeParser dataTypeParser,
            IEnumerable<ILanguageProvider> languageProviders,
            IModelRepository modelRepository,
            IConsole console) : base(console)
        {
            _dataTypeParser = dataTypeParser;
            _modelRepository = modelRepository;
            _codeLanguageProvider = languageProviders.Single(e => e.Name.IsSame("cs"));
            _databaseLanguageProvider = languageProviders.Single(e => e.Name.IsSame("SqlServer"));
        }

        public override Task Visit(EntityDeclarationNode node)
        {
            var model = new EntityModel(node.Name);
            _modelRepository.Add(model);
            return base.Visit(node);
        }

        public override Task Visit(AttributeDeclarationNode node)
        {
            var result = _dataTypeParser.Parse(node.AttributeDataType);

            if (_modelRepository.TryGet(node.Parent.Name, out var entityModel))
            {
                var dataType = DataTypeFactory.Create(result.DataType);

                var codeDataType = _codeLanguageProvider.Translate(dataType);
                var databaseDataType = _databaseLanguageProvider.Translate(dataType);

                entityModel.CodeAttributes.Add(new AttributeModel(node.AttributeName, dataType, codeDataType, result.IsNullable, result.DefaultValue));
                entityModel.DataAttributes.Add(new AttributeModel(node.AttributeName, dataType, databaseDataType, result.IsNullable, result.DefaultValue));
            }

            return base.Visit(node);
        }

        public override Task Visit(AbstractDeclarationNode node)
        {
            var entityModel = _modelRepository.Get(node.Parent.Name);
            entityModel.IsAbstract = node.Value;

            return base.Visit(node);
        }
    }
}
