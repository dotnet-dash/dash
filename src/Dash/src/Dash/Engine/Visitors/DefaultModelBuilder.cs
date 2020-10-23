// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dash.Common;
using Dash.Engine.Models;
using Dash.Extensions;
using Dash.Nodes;

namespace Dash.Engine.Visitors
{
    public class DefaultModelBuilder : BaseVisitor
    {
        private readonly IDataTypeDeclarationParser _dataTypeDeclarationParser;
        private readonly IModelRepository _modelRepository;
        private readonly ILanguageProvider _codeLanguageProvider;
        private readonly ILanguageProvider _databaseLanguageProvider;

        public DefaultModelBuilder(
            IDataTypeDeclarationParser dataTypeDeclarationParser,
            IEnumerable<ILanguageProvider> languageProviders,
            IModelRepository modelRepository,
            IConsole console) : base(console)
        {
            _dataTypeDeclarationParser = dataTypeDeclarationParser;
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
            var result = _dataTypeDeclarationParser.Parse(node.AttributeDataType);

            if (_modelRepository.TryGet(node.Parent.Name, out var entityModel))
            {
                var codeDataType = _codeLanguageProvider.Translate(result.DataType);
                var databaseDataType = _databaseLanguageProvider.Translate(result.DataType);

                entityModel.CodeAttributes.Add(new AttributeModel(node.AttributeName, result, codeDataType));
                entityModel.DataAttributes.Add(new AttributeModel(node.AttributeName, result, databaseDataType));
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
