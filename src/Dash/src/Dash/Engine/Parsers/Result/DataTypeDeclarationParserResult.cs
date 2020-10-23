// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Dash.Engine.Parsers.Result
{
    public class DataTypeDeclarationParserResult
    {
        public DataTypeDeclarationParserResult(IDataType dataType)
        {
            DataType = dataType;
        }

        public IDataType DataType { get; }

        public bool IsNullable { get; private set; }

        public string? DefaultValue { get; private set; }

        public string? RegularExpression { get; private set; }

        public int? MaxLength { get; private set; }

        public int? RangeLowerBound { get; set; }

        public int? RangeUpperBound { get; set; }

        public DataTypeDeclarationParserResult WithIsNullable(bool value)
        {
            IsNullable = value;
            return this;
        }

        public DataTypeDeclarationParserResult WithDefaultValue(string defaultValue)
        {
            DefaultValue = defaultValue;
            return this;
        }

        public DataTypeDeclarationParserResult WithRegularExpression(string regularExpression)
        {
            RegularExpression = regularExpression;
            return this;
        }

        public DataTypeDeclarationParserResult WithMaximumLength(int maxLength)
        {
            MaxLength = maxLength;
            return this;
        }
    }
}