// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Dash.Application;
using Dash.Common;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.Options;

namespace Dash.Engine.Generator
{
    [ExcludeFromCodeCoverage]
    public class EditorConfigCodeFormatter : IPostGenerator
    {
        private readonly IConsole _console;
        private readonly IBuildOutputRepository _buildOutputRepository;
        private readonly IErrorRepository _errorRepository;
        private readonly DashOptions _options;

        public EditorConfigCodeFormatter(
            IConsole console,
            IBuildOutputRepository buildOutputRepository,
            IOptions<DashOptions> dashOptions,
            IErrorRepository errorRepository)
        {
            _console = console;
            _buildOutputRepository = buildOutputRepository;
            _errorRepository = errorRepository;
            _options = dashOptions.Value;
        }

        public async Task Run()
        {
            _console.Info("Format code according to existing .editorconfig in project directory");

            MSBuildLocator.RegisterDefaults();
            using var workspace = MSBuildWorkspace.Create();

            _console.Trace($"Opening project file '{_options.ProjectFile}'");
            var project = await workspace.OpenProjectAsync(_options.ProjectFile);
            if (project == null)
            {
                _errorRepository.Add("Unable to open project file");
                return;
            }

            var buildOutputs = _buildOutputRepository.GetOutputItems().ToList();
            var newDocumentIds = new List<DocumentId>();

            _console.Trace("Adding build outputs to project");
            foreach (var buildOutput in buildOutputs)
            {
                var newDocument = project.AddDocument(buildOutput.Path, buildOutput.GeneratedSourceCodeContent, null, buildOutput.Path);
                project = newDocument.Project;

                newDocumentIds.Add(newDocument.Id);
            }

            var documents = newDocumentIds
                .Select(documentId => project.Solution.GetDocument(documentId))
                .Where(e => e != null)
                .Select(e => e!);

            foreach (var document in documents)
            {
                _console.Trace($"Formatting document {document.FilePath}");
                var formattedDocument = await Formatter.FormatAsync(document);
                var formattedSourceCode = await formattedDocument.GetTextAsync();
                _buildOutputRepository.Update(new BuildOutput(document.FilePath!, formattedSourceCode.ToString()));
            }
        }
    }
}
