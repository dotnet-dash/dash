using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Dash.Common.Abstractions;
using Dash.Engine.Abstractions;
using Dash.Extensions;

namespace Dash.Engine.Repositories
{
    public class UriResourceRepository : IUriResourceRepository
    {
        private readonly IConsole _console;
        private readonly IFileSystem _fileSystem;
        private readonly IClock _clock;
        private readonly IEmbeddedTemplateProvider _embeddedTemplateProvider;
        private readonly IDictionary<Uri, string> _resources = new Dictionary<Uri, string>();

        public UriResourceRepository(
            IConsole console,
            IFileSystem fileSystem,
            IClock clock,
            IEmbeddedTemplateProvider embeddedTemplateProvider)
        {
            _console = console;
            _fileSystem = fileSystem;
            _clock = clock;
            _embeddedTemplateProvider = embeddedTemplateProvider;
        }

        public async Task Add(Uri uriResource)
        {
            var result = await Exists(uriResource);
            if (result)
            {
                return;
            }

            var value = uriResource.Scheme.IsSame("dash")
                ? uriResource.Host
                : uriResource.AbsolutePath;

            _resources.Add(uriResource, value);
        }

        public async Task Add(Uri uriResource, string fileName, byte[] contents)
        {
            var temporaryFile = GetTempPath(fileName);

            await _fileSystem.File.WriteAllBytesAsync(temporaryFile, contents);
            _console.Trace($"Resource '{uriResource}' saved to '{temporaryFile}'");

            _resources.Add(uriResource, temporaryFile);
        }

        public Task<string> Get(Uri uriResource)
        {
            return Task.FromResult(_resources[uriResource]);
        }

        public Task<bool> Exists(Uri uriResource)
        {
            return Task.FromResult(_resources.TryGetValue(uriResource, out _));
        }

        public Task<string> GetContents(Uri uriResource)
        {
            var fileName = _resources[uriResource];

            if (uriResource.Scheme.IsSame("dash"))
            {
                return _embeddedTemplateProvider.GetTemplate(fileName);
            }

            return _fileSystem.File.ReadAllTextAsync(fileName);
        }

        private string GetTempPath(string fileName)
        {
            var tempDash = Path.Combine(_fileSystem.Path.GetTempPath(), "dash", _clock.UtcNow.Ticks.ToString());

            if (!_fileSystem.Directory.Exists(tempDash))
            {
                _fileSystem.Directory.CreateDirectory(tempDash);
            }

            return Path.Combine(tempDash, fileName);
        }
    }
}
