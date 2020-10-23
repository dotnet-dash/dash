// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Dash.Engine.Parsers.Result
{
    public class DataTypeParserResult // TODO: rename to something that appropriately covers the concept.
    {
        public DataTypeParserResult(IDataType dataType)
        {
            DataType = dataType;
        }

        public IDataType DataType { get; }

        public bool IsNullable { get; private set; }

        public string? DefaultValue { get; private set; }

        public string? DataTypeRegularExpression { get; set; }

        public int? Length { get; set; }

        public int? RangeLowerBound { get; set; }

        public int? RangeUpperBound { get; set; }

        public DataTypeParserResult WithIsNullable(bool value)
        {
            IsNullable = value;
            return this;
        }

        public DataTypeParserResult WithDefaultValue(string? defaultValue)
        {
            DefaultValue = defaultValue;
            return this;
        }
    }
}