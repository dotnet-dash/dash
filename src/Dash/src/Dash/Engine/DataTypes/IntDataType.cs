// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Dash.Engine.DataTypes
{
    public class IntDataType : IDataType
    {
        public string Name => "int";
        public bool IsNumeric => true;
        public bool IsDateTime => false;
        public bool IsBoolean => false;

        public static IntDataType Default => new IntDataType();
    }
}
