namespace Dash.Common
{
    public interface ISessionService
    {
        string GetTempPath(string fileName);

        string GetWorkingDirectory();
    }
}