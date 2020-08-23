using System;

namespace Dash.Common
{
    public interface IClock
    {
        DateTime UtcNow { get; }
    }
}