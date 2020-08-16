namespace Dash.Engine.Abstractions
{
    public interface IConsole
    {
        void WriteLine(string message);
        void WriteError(string errorMessage);
    }
}