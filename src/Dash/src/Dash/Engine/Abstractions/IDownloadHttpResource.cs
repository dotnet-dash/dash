using System;
using System.Threading.Tasks;

namespace Dash.Engine.Abstractions
{
    public interface IDownloadHttpResource
    {
        Task<(bool Success, string? FileName, byte[]? Content)> Download(Uri uri);
    }
}