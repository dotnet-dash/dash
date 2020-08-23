using System;
using Dash.Application;
using Dash.Common.Abstractions;
using Microsoft.Extensions.Options;

namespace Dash.Common
{
    public class DefaultConsole : IConsole
    {
        private readonly DashOptions _dashOptions;

        public DefaultConsole(IOptions<DashOptions> dashOptions)
        {
            _dashOptions = dashOptions.Value;
        }

        public void Trace(string message)
        {
            if (_dashOptions.Verbose)
            {
                Console.Out.WriteLine(message);
            }
        }

        public void Info(string message)
        {
            Console.Out.WriteLine(message);
        }

        public void Error(string errorMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(errorMessage);
            Console.ResetColor();
        }
    }
}
