// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Dash.Engine.Models
{
    public class AttributeModel
    {
        public AttributeModel(string name, string dataType, bool isNullable, string? defaultValue)
        {
            Name = name;
            DataType = dataType;
            IsNullable = isNullable;
            DefaultValue = defaultValue;
        }

        public string Name { get; }

        public string DataType { get; }

        public string? DefaultValue { get; }

        public bool IsNullable { get; }
    }
}