// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Dash.Common;
using Dash.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Formatting;

namespace Dash.Engine.Generator
{
    [ExcludeFromCodeCoverage]
    public class EditorConfigCodeFormatter : IPostGenerator
    {
        private readonly IConsole _console;
        private readonly IBuildOutputRepository _buildOutputRepository;
        private readonly IWorkspace _workspace;

        public EditorConfigCodeFormatter(
            IConsole console,
            IBuildOutputRepository buildOutputRepository,
            IWorkspace workspace)
        {
            _console = console;
            _buildOutputRepository = buildOutputRepository;
            _workspace = workspace;
        }

        public async Task Run()
        {
            _console.Info("Format code according to existing .editorconfig in project directory");

            var project = await _workspace.OpenProjectAsync();
            if (project == null)
            {
                return;
            }

            var buildOutputs = _buildOutputRepository.GetOutputItems().ToList();
            var newDocumentIds = new List<DocumentId>();

            _console.Trace("Adding build outputs to project");
            foreach (var buildOutput in buildOutputs)
            {
                var newDocument = project.AddDocument(buildOutput.Path, buildOutput.GeneratedSourceCodeContent);
                newDocumentIds.Add(newDocument.DocumentId);
            }

            var documents = newDocumentIds
                .Select(documentId => project.GetDocument(documentId))
                .Select(e => e!);

            foreach (var document in documents)
            {
                _console.Trace($"Formatting document {document.FilePath}");
                var formattedDocument = await Formatter.FormatAsync(document.Document);
                var formattedSourceCode = await formattedDocument.GetTextAsync();
                _buildOutputRepository.Update(new BuildOutput(document.FilePath!, formattedSourceCode.ToString()));
            }
        }
    }
}
