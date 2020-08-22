using System;
using Dash.Common.Abstractions;

namespace Dash.Common
{
    public class Clock : IClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
