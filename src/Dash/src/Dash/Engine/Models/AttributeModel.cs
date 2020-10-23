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
        private readonly DataTypeParserResult _parsedDashDataType;

        public AttributeModel(string name, DataTypeParserResult parserResult, string dataType)
        {
            Name = name;
            _parsedDashDataType = parserResult;
            DataType = dataType;
        }

        public string Name { get; }

        public IDataType DashDataType => _parsedDashDataType.DataType;

        public string? DefaultValue => _parsedDashDataType.DefaultValue;

        public string? RegularExpression => _parsedDashDataType.DataTypeRegularExpression;

        public bool IsNullable => _parsedDashDataType.IsNullable;

        public int? Length => _parsedDashDataType.Length;

        public int? RangeLowerBound => _parsedDashDataType.RangeLowerBound;

        public int? RangeUpperBound => _parsedDashDataType.RangeUpperBound;

        public string DataType { get; }
    }
}