// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Dash.Engine.DataTypes
{
    public class DateTimeDataType : IDataType
    {
        public string Name => "DateTime";
        public bool IsNumeric => false;
        public bool IsDateTime => true;
        public bool IsBoolean => false;
        public bool IsUnicode => false;
        public bool IsString => false;
    }
}
