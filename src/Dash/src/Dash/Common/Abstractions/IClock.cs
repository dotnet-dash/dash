using System;

namespace Dash.Common.Abstractions
{
    public interface IClock
    {
        DateTime UtcNow { get; }
    }
}