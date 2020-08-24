// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using Dash.Engine;

namespace Dash.Nodes
{
    public abstract class AstNode
    {
        public abstract Task Accept(INodeVisitor visitor);
    }
}