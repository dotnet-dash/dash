// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.CodeAnalysis;

namespace Dash.Roslyn.Default
{
    public class ProjectFacade : IProject
    {
        private Project _project;

        public string Namespace => _project.DefaultNamespace!;

        public ProjectFacade(Project project)
        {
            _project = project;
        }

        public IDocument AddDocument(string path, string text)
        {
            var addedDocument = _project.AddDocument(path, text, null, path);
            _project = addedDocument.Project;
            return new DocumentFacade(addedDocument);
        }

        public IDocument GetDocument(DocumentId documentId)
        {
            return new DocumentFacade(_project.GetDocument(documentId)!);
        }
    }
}
