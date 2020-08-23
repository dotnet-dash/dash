using System;
using System.Threading.Tasks;

namespace Dash.Common
{
    public interface IHttpUriDownloader
    {
        Task<(bool Success, string? FileName, byte[]? Content)> Download(Uri uri);
    }
}