using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dash.Engine.Abstractions;
using Dash.Nodes;

namespace Dash.Engine.Visitors
{
    public class UriResourceDownload : BaseVisitor
    {
        private readonly IFileSystem _fileSystem;
        private readonly IErrorRepository _errorRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly string[] _supportedSchemes = {"http", "https"};

        public UriResourceDownload(IFileSystem fileSystem, IConsole console, IErrorRepository errorRepository, IHttpClientFactory httpClientFactory) : base(console)
        {
            _fileSystem = fileSystem;
            _errorRepository = errorRepository;
            _httpClientFactory = httpClientFactory;
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

                    var temporaryFileName = string.Join("_", fileName, DateTime.UtcNow.Ticks);

                    node.LocalCopy = GetTempPath(temporaryFileName);

                    Console.Trace($"Contents saved to {node.LocalCopy}");
                    await _fileSystem.File.WriteAllBytesAsync(node.LocalCopy, bytes);
                }
                else
                {
                    _errorRepository.Add($"Error while downloading {node.Uri}");
                }
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
