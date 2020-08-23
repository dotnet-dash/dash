using System;

namespace Dash.Application
{
    public interface IApplicationServiceProvider
    {
        IServiceProvider Create(bool verbose, string? outputDir);
    }
}