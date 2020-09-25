// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Threading.Tasks;
using Dash.Constants;
using Dash.Engine;

namespace Dash.Nodes
{
    public class UriNode : AstNode
    {
        public static UriNode ForExistingFile(Uri uri) => new UriNode(uri, true, new[]
        {
            Uri.UriSchemeFile
        });

        public static UriNode ForOutputFile(Uri uri) => new UriNode(uri, false, new []
        {
            Uri.UriSchemeFile
        });

        public static UriNode ForExternalResources(Uri uri) => new UriNode(uri, true, new[]
        {
            Uri.UriSchemeFile,
            Uri.UriSchemeHttp,
            Uri.UriSchemeHttps,
        });

        public static UriNode ForInternalExternalResources(Uri uri) => new UriNode(uri, true, new []
        {
            Uri.UriSchemeFile,
            Uri.UriSchemeHttp,
            Uri.UriSchemeHttps,
            DashModelFileConstants.UriSchemeDash,
        });

        private UriNode(Uri uri, bool uriMustExist, string[] supportedSchemes)
        {
            Uri = uri;
            UriMustExist = uriMustExist;
            SupportedSchemes = supportedSchemes;
        }

        public Uri Uri { get; }
        public bool UriMustExist { get; }
        public string[] SupportedSchemes { get; set; }

        public override async Task Accept(INodeVisitor visitor)
        {
            await visitor.Visit(this);
        }
    }
}
