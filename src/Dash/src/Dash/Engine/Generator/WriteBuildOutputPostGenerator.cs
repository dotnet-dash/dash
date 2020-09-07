// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Dash.Common;

namespace Dash.Engine.Generator
{
    public class WriteBuildOutputPostGenerator : IPostGenerator
    {
        private readonly IConsole _console;
        private readonly IBuildOutputRepository _buildOutputRepository;
        private readonly IFileSystem _fileSystem;

        public WriteBuildOutputPostGenerator(
            IConsole console,
            IBuildOutputRepository buildOutputRepository,
            IFileSystem fileSystem)
        {
            _console = console;
            _buildOutputRepository = buildOutputRepository;
            _fileSystem = fileSystem;
        }

        public async Task Run()
        {
            var filesWritten = 0;

            foreach (var item in _buildOutputRepository.GetOutputItems())
            {
                var directory = Path.GetDirectoryName(item.Path);

                if (!_fileSystem.Directory.Exists(directory))
                {
                    _console.Trace($"Directory {directory} does not exist. Creating...");
                    _fileSystem.Directory.CreateDirectory(directory);
                }

                _console.Trace($"Writing {item.Path} to disk");
                await _fileSystem.File.WriteAllTextAsync(item.Path, item.GeneratedSourceCodeContent);
                filesWritten++;
            }

            _console.Info($"{filesWritten} file(s) written to filesystem");
        }
    }
}
