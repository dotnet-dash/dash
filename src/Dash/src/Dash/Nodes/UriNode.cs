// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Threading.Tasks;
using Dash.Engine;

namespace Dash.Nodes
{
    public class UriNode : AstNode
    {
        public Uri Uri { get; }

        public UriNode(Uri uri, bool uriMustExist)
        {
            Uri = uri;
            UriMustExist = uriMustExist;
        }

        public bool UriMustExist { get; }

        public override async Task Accept(INodeVisitor visitor)
        {
            await visitor.Visit(this);
        }
    }
}
