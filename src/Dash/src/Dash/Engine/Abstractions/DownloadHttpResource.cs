using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dash.Engine.Abstractions
{
    public class DownloadHttpResource : IDownloadHttpResource
    {
        private readonly IConsole _console;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IErrorRepository _errorRepository;

        public DownloadHttpResource(IConsole console,
            IHttpClientFactory httpClientFactory,
            IErrorRepository errorRepository)
        {
            _console = console;
            _httpClientFactory = httpClientFactory;
            _errorRepository = errorRepository;
        }

        public async Task<(bool Success, string? FileName, byte[]? Content)> Download(Uri uri)
        {
            _console.Trace($"Downloading URI {uri}");

            var httpResponseMessage = await _httpClientFactory.CreateClient().GetAsync(uri);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var bytes = await httpResponseMessage.Content.ReadAsByteArrayAsync();

                var fileName = httpResponseMessage.Content.Headers.ContentDisposition?.FileName
                               ?? Path.GetFileName(uri.LocalPath);

                _console.Info($"Download '{uri}' successful");
                return (true, fileName, bytes);
            }

            _errorRepository.Add($"Error while downloading '{uri}'. Status code == {httpResponseMessage.StatusCode}");
            return (false, null, null);
        }
    }
}