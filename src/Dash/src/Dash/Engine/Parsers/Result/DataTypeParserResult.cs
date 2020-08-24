// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Dash.Engine.Parsers.Result
{
    public class DataTypeParserResult
    {
        public DataTypeParserResult(string dataType)
        {
            DataType = dataType;
        }

        public string DataType { get; }

        public bool IsNullable { get; set; }

        public string? DefaultValue { get; set; }

        public string? DataTypeRegularExpression { get; set; }

        public int? Length { get; set; }

        public int? RangeLowerBound { get; set; }

        public int? RangeUpperBound { get; set; }
    }
}