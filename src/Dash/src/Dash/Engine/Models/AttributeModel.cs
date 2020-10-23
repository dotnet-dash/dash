// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Dash.Engine.Parsers.Result;

namespace Dash.Engine.Models
{
    /// <summary>
    /// This class represents an Attribute and could be consumed by an <see cref="ITemplateTransformer"/>.
    /// </summary>
    public class AttributeModel
    {
        private readonly DataTypeDeclarationParserResult _parsedDashDataTypeDeclaration;

        public AttributeModel(string name, DataTypeDeclarationParserResult declarationParserResult, string targetEnvironmentDataType)
        {
            Name = name;
            _parsedDashDataTypeDeclaration = declarationParserResult;
            TargetEnvironmentDataType = targetEnvironmentDataType;
        }

        public string Name { get; }

        public IDataType DataType => _parsedDashDataTypeDeclaration.DataType;

        public string? DefaultValue => _parsedDashDataTypeDeclaration.DefaultValue;

        public string? RegularExpression => _parsedDashDataTypeDeclaration.RegularExpression;

        public bool IsNullable => _parsedDashDataTypeDeclaration.IsNullable;

        public int? MaxLength => _parsedDashDataTypeDeclaration.MaxLength;

        public int? RangeLowerBound => _parsedDashDataTypeDeclaration.RangeLowerBound;

        public int? RangeUpperBound => _parsedDashDataTypeDeclaration.RangeUpperBound;

        public string TargetEnvironmentDataType { get; }
    }
}