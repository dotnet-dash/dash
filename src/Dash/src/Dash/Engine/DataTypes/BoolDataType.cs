﻿// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Dash.Engine.DataTypes
{
    public class BoolDataType : IDataType
    {
        public string Name => "bool";
        public bool IsNumeric => false;
        public bool IsDateTime => false;
        public bool IsBoolean => true;
        public bool IsString => false;
        public bool IsUnicode => false;
    }
}
