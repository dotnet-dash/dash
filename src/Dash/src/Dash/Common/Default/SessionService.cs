// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.IO;
using System.IO.Abstractions;
using Dash.Application;
using Microsoft.Extensions.Options;

namespace Dash.Common.Default
{
    public class SessionService : ISessionService
    {
        private readonly IFileSystem _fileSystem;
        private readonly IClock _clock;
        private readonly DashOptions _options;

        public SessionService(IFileSystem fileSystem, IClock clock, IOptions<DashOptions> options)
        {
            _fileSystem = fileSystem;
            _clock = clock;
            _options = options.Value;
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

        public string GetWorkingDirectory()
        {
            return _options.OutputDirectory ?? _fileSystem.Directory.GetCurrentDirectory();
        }
    }
}
