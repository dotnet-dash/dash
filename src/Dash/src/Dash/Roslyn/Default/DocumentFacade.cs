// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.CodeAnalysis;

namespace Dash.Roslyn.Default
{
    public class DocumentFacade : IDocument
    {
        public Document Document { get; }

        public DocumentFacade(Document document)
        {
            Document = document;
        }

        public DocumentId DocumentId => Document.Id;

        public string FilePath => Document.FilePath!;
    }
}