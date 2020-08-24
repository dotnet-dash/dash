// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Dash.Common;
using Dash.Extensions;

namespace Dash.Engine.Repositories
{
    public class UriResourceRepository : IUriResourceRepository
    {
        private readonly IConsole _console;
        private readonly IFileSystem _fileSystem;
        private readonly ISessionService _sessionService;
        private readonly IEmbeddedTemplateProvider _embeddedTemplateProvider;
        private readonly IDictionary<Uri, string> _resources = new Dictionary<Uri, string>();

        public UriResourceRepository(
            IConsole console,
            IFileSystem fileSystem,
            ISessionService sessionService,
            IEmbeddedTemplateProvider embeddedTemplateProvider)
        {
            _console = console;
            _fileSystem = fileSystem;
            _sessionService = sessionService;
            _embeddedTemplateProvider = embeddedTemplateProvider;
        }

        public async Task Add(Uri uri)
        {
            var result = await Exists(uri);
            if (result)
            {
                return;
            }

            _resources.Add(uri, uri.ToString());
        }

        public async Task Add(Uri uri, string fileName, byte[] contents)
        {
            var temporaryFile = _sessionService.GetTempPath(fileName);

            await _fileSystem.File.WriteAllBytesAsync(temporaryFile, contents);
            _console.Trace($"Resource '{uri}' saved to '{temporaryFile}'");

            _resources.Add(uri, temporaryFile);
        }

        public Task<string> Get(Uri uri)
        {
            if (_resources.TryGetValue(uri, out string? value))
            {
                return Task.FromResult(value!);
            }

            throw new InvalidOperationException($"Uri '{uri}' not in repository");
        }

        public Task<bool> Exists(Uri uri)
        {
            return Task.FromResult(_resources.TryGetValue(uri, out _));
        }

        public async Task<string> GetContents(Uri uri)
        {
            var fileName = await Get(uri);

            if (uri.Scheme.IsSame("dash"))
            {
                return await _embeddedTemplateProvider.GetTemplate(uri.Host);
            }

            return await _fileSystem.File.ReadAllTextAsync(fileName);
        }

        public Task<int> Count()
        {
            return Task.FromResult(_resources.Count);
        }
    }
}
