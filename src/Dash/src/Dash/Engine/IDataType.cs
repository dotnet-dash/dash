// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Dash.Engine
{
    /// <summary>
    /// Abstract representation of a Dash Data Type defined in the Model File.
    /// </summary>
    public interface IDataType
    {
        public string Name { get; }

        public bool IsNumeric { get; }

        public bool IsDateTime { get; }

        bool IsBoolean { get; }

        bool IsUnicode { get; }

        bool IsString { get; }
    }
}
