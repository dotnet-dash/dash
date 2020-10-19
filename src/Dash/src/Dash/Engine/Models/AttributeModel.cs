// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Dash.Engine.Models
{
    /// <summary>
    /// This class represents an Attribute and could be consumed by an <see cref="ITemplateTransformer"/>.
    /// </summary>
    public class AttributeModel
    {
        public AttributeModel(string name, IDataType dashDataType, string dataType, bool isNullable, string? defaultValue)
        {
            Name = name;
            DashDataType = dashDataType;
            DataType = dataType;
            IsNullable = isNullable;
            DefaultValue = defaultValue;
        }

        public string Name { get; }

        public IDataType DashDataType { get; }

        public string DataType { get; }

        public string? DefaultValue { get; }

        public bool IsNullable { get; }
    }
}