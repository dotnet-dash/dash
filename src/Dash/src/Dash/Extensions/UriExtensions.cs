using System;
using System.IO;
using Dash.Common;

namespace Dash.Extensions
{
    public static class UriExtensions
    {
        public static string ToPath(this Uri uri, ISessionService sessionService)
        {
            if (uri.IsAbsoluteUri)
            {
                return uri.AbsolutePath;
            }

            return Path.Combine(sessionService.GetWorkingDirectory(), uri.ToString());
        }
    }
}
