// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dash.Engine;

namespace Dash.Nodes
{
    public class CsvSeedDeclarationNode : AstNode
    {
        public EntityDeclarationNode Parent { get; }
        public UriNode UriNode { get; }
        public bool FirstLineIsHeader { get; }
        public IDictionary<string, string> MapHeaders { get; }
        public string Delimiter { get; }

        public CsvSeedDeclarationNode(EntityDeclarationNode parent, Uri uri, bool firstLineIsHeader, string? delimiter, IDictionary<string, string> mapHeaders)
        {
            Parent = parent;
            UriNode = UriNode.ForExternalResources(uri);
            FirstLineIsHeader = firstLineIsHeader;
            Delimiter = delimiter ?? ",";
            MapHeaders = mapHeaders;
        }

        public override async Task Accept(INodeVisitor visitor)
        {
            await visitor.Visit(this);
        }
    }
}
