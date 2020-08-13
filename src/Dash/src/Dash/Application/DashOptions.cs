namespace Dash.Application
{
    public class DashOptions
    {
        public string OutputDirectory { get; set; } = ".";

        public bool Verbose { get; set; }

        public string[] Templates { get; set; } = { "Poco" };
    }
}
