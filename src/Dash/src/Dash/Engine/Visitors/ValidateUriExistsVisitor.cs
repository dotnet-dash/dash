﻿// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.IO.Abstractions;
using System.Threading.Tasks;
using Dash.Common;
using Dash.Extensions;
using Dash.Nodes;

namespace Dash.Engine.Visitors
{
    public class ValidateUriExistsVisitor : BaseVisitor
    {
        private readonly IFileSystem _fileSystem;
        private readonly IErrorRepository _errorRepository;
        private readonly IEmbeddedTemplateProvider _embeddedTemplateProvider;

        public ValidateUriExistsVisitor(
            IConsole console,
            IFileSystem fileSystem,
            IErrorRepository errorRepository,
            IEmbeddedTemplateProvider embeddedTemplateProvider) : base(console)
        {
            _fileSystem = fileSystem;
            _errorRepository = errorRepository;
            _embeddedTemplateProvider = embeddedTemplateProvider;
        }

        public override async Task Visit(UriNode node)
        {
            ValidateFileExists(node);
            await ValidateDashResourceExists(node);

            await base.Visit(node);
        }

        private void ValidateFileExists(UriNode node)
        {
            if (node.UriMustExist &&
                node.Uri.Scheme.IsSame("file") &&
                !_fileSystem.File.Exists(node.Uri.AbsolutePath))
            {
                _errorRepository.Add($"File does not exist: '{node.Uri}'");
            }
        }

        private async Task ValidateDashResourceExists(UriNode node)
        {
            if (node.UriMustExist &&
                node.Uri.Scheme.IsSame("dash") &&
                !(await _embeddedTemplateProvider.Exists(node.Uri.Host)))
            {
                _errorRepository.Add($"Dash template does not exist: {node.Uri}");
            }
        }
    }
}
