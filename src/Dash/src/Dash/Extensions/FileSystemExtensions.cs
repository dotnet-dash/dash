// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.IO;
using System.IO.Abstractions;
using Dash.Application;

namespace Dash.Extensions
{
    public static class FileSystemExtensions
    {
        public static string GetAbsoluteWorkingDirectory(this IFileSystem fileSystem, DashOptions dashOptions)
        {
            if (dashOptions.WorkingDirectory == ".")
            {
                return fileSystem.Directory.GetCurrentDirectory();
            }

            if (dashOptions.WorkingDirectory == "..")
            {
                var currentDirectory = fileSystem.Directory.GetCurrentDirectory();
                var directoryInfo = fileSystem.DirectoryInfo.FromDirectoryName(currentDirectory);
                return directoryInfo.Parent.FullName;
            }

            return fileSystem.DirectoryInfo.FromDirectoryName(dashOptions.WorkingDirectory).FullName;
        }

        public static string AbsolutePath(this IFileSystem fileSystem, Uri uri, DashOptions dashOptions)
        {
            if (uri.IsAbsoluteUri)
            {
                return fileSystem.DirectoryInfo.FromDirectoryName(uri.LocalPath).FullName;
            }

            var combinedPath = Path.Combine(fileSystem.GetAbsoluteWorkingDirectory(dashOptions), uri.ToString());

            return fileSystem.DirectoryInfo.FromDirectoryName(combinedPath).FullName;
        }
    }
}
