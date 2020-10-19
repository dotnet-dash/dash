// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using Dash.Extensions;

namespace Dash.Engine.DataTypes
{
    public static class DataTypeFactory
    {
        private static IDataType[] DashDataTypes => new IDataType[]
        {
            new StringDataType(),
            new IntDataType(),
            new BoolDataType(),
            new DateTimeDataType(),
            new UnicodeDataType(),
            new GuidDataType(),
            new EmailDataType(),
        };

        public static IDataType Create(string dashDataType)
        {
            foreach (var item in DashDataTypes)
            {
                if (item.Name.IsSame(dashDataType))
                {
                    return item;
                }
            }

            throw new InvalidOperationException($"Unknown Dash data type '{dashDataType}'");
        }
    }
}
