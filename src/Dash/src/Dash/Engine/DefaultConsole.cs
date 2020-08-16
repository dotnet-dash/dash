using System;
using Dash.Engine.Abstractions;

namespace Dash.Engine
{
    public class DefaultConsole : IConsole
    {
        public void WriteLine(string message)
        {
            Console.Out.WriteLine(message);
        }

        public void WriteError(string errorMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(errorMessage);
            Console.ResetColor();
        }
    }
}
