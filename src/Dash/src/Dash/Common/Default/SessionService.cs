// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.IO;
using System.IO.Abstractions;
using System.Linq;
using Dash.Application;
using Dash.Engine;
using Dash.Extensions;
using Microsoft.Extensions.Options;

namespace Dash.Common.Default
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

            return Path.Combine(tempDash, fileName).NormalizeSlashes();
        }

        //public string GetProjectFile()

#pragma warning disable S125 // Sections of code should not be commented out
                            //{
                            //    if (_options.ProjectFile == null)
                            //    {
                            //        _console.Info("No .csproj specified. Finding .csproj");

        //        var path = _fileSystem.GetAbsoluteWorkingDirectory(_options);

        //        var projectFiles = _fileSystem.Directory.GetFiles(path, "*.csproj");
        //        if (projectFiles.Length == 0)
        //        {
        //            _errorRepository.Add("No .csproj file no found");
        //        }
        //        else if (projectFiles.Length > 1)
        //        {
        //            _errorRepository.Add("Multiple .csproj files found.");
        //        }
        //        else
        //        {
        //            return projectFiles.First();
        //        }
        //    }
        //    else
        //    {
        //        return _options.ProjectFile;
        //    }
        //}
    }
#pragma warning restore S125 // Sections of code should not be commented out
}
