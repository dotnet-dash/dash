// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Dash.Engine.Parsers.Result;

namespace Dash.Engine
{
    public interface IDataTypeParser
    {
        DataTypeParserResult Parse(string dataTypeSpecification);
    }
}