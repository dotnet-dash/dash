using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dash.Common.Abstractions;
using Dash.Engine.Abstractions;
using Dash.Nodes;

namespace Dash.Engine.Visitors
{
    public class UriResourceDownload : BaseVisitor
    {
        private readonly IFileSystem _fileSystem;
        private readonly IErrorRepository _errorRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IClock _clock;

        private readonly string[] _supportedSchemes = {"http", "https"};

        public UriResourceDownload(
            IFileSystem fileSystem,
            IConsole console,
            IErrorRepository errorRepository,
            IHttpClientFactory httpClientFactory,
            IClock clock) : base(console)
        {
            _fileSystem = fileSystem;
            _errorRepository = errorRepository;
            _httpClientFactory = httpClientFactory;
            _clock = clock;
        }

        public override async Task Visit(UriNode node)
        {
            if (_supportedSchemes.Contains(node.Uri.Scheme, StringComparer.OrdinalIgnoreCase))
            {
                Console.Trace($"Downloading URI {node.Uri}");

                var httpResponseMessage = await _httpClientFactory.CreateClient().GetAsync(node.Uri);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var bytes = await httpResponseMessage.Content.ReadAsByteArrayAsync();

                    var fileName = httpResponseMessage.Content.Headers.ContentDisposition?.FileName
                                   ?? Path.GetFileName(node.Uri.LocalPath);

                    var temporaryFileName = string.Join("_", _clock.UtcNow.Ticks, fileName);

                    node.LocalCopy = GetTempPath(temporaryFileName);

                    Console.Trace($"Contents saved to {node.LocalCopy}");
                    await _fileSystem.File.WriteAllBytesAsync(node.LocalCopy, bytes);
                }
                else
                {
                    _errorRepository.Add($"Error while downloading '{node.Uri}'. Status code == {httpResponseMessage.StatusCode}");
                }
            }
            else
            {
                _errorRepository.Add($"Unsupported scheme '{node.Uri.Scheme}' found in '{node.Uri}'");
            }

            await base.Visit(node);
        }

        private string GetTempPath(string fileName)
        {
            var tempDash = Path.Combine(_fileSystem.Path.GetTempPath(), "dash");

            if (!_fileSystem.Directory.Exists(tempDash))
            {
                _fileSystem.Directory.CreateDirectory(tempDash);
            }

            return Path.Combine(tempDash, fileName);
        }
    }
}
