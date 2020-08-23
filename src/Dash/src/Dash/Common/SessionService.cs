using System.IO;
using System.IO.Abstractions;
using Dash.Common.Abstractions;

namespace Dash.Common
{
    public class SessionService : ISessionService
    {
        private readonly IFileSystem _fileSystem;
        private readonly IClock _clock;

        public SessionService(IFileSystem fileSystem, IClock clock)
        {
            _fileSystem = fileSystem;
            _clock = clock;
        }

        public string GetTempPath(string fileName)
        {
            var tempDash = Path.Combine(_fileSystem.Path.GetTempPath(), "dash", _clock.UtcNow.Ticks.ToString());

            if (!_fileSystem.Directory.Exists(tempDash))
            {
                _fileSystem.Directory.CreateDirectory(tempDash);
            }

            return Path.Combine(tempDash, fileName);
        }
    }
}
