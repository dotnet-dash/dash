// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.CodeAnalysis;

namespace Dash.Roslyn
{
    public interface IDocument
    {
        DocumentId DocumentId { get; }

        Document Document { get; }

        string FilePath { get; }
    }
}