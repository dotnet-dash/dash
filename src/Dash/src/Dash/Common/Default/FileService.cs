// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.IO.Abstractions;
using Dash.Application;
using Dash.Extensions;
using Microsoft.Extensions.Options;

namespace Dash.Common.Default
{
    public class FileService : IFileService
    {
        private readonly IFileSystem _fileSystem;
        private readonly DashOptions _dashOptions;

        public FileService(IFileSystem fileSystem, IOptions<DashOptions> dashOptions)
        {
            _fileSystem = fileSystem;
            _dashOptions = dashOptions.Value;
        }

        public string AbsoluteWorkingDirectory => _fileSystem.GetAbsoluteWorkingDirectory(_dashOptions);
    }
}
