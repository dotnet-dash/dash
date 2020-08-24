// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Dash.Nodes;

namespace Dash.Engine
{
    public interface ISourceCodeParser
    {
        SourceCodeNode Parse(string sourceCode);
    }
}
