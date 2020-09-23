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
            if (dashOptions.Project == null)
            {
                return fileSystem.Directory.GetCurrentDirectory();
            }

            var fileName = Path.GetFileName(dashOptions.Project);
            if (string.Equals(fileName, dashOptions.Project, StringComparison.InvariantCultureIgnoreCase))
            {
                return fileSystem.Directory.GetCurrentDirectory();
            }

            var directoryName = Path.GetDirectoryName(dashOptions.Project);
            return fileSystem.DirectoryInfo.FromDirectoryName(directoryName).FullName;
        }

        public static string AbsolutePath(this IFileSystem fileSystem, Uri relativeUri, DashOptions dashOptions)
        {
            if (relativeUri.IsAbsoluteUri)
            {
                return fileSystem.DirectoryInfo.FromDirectoryName(relativeUri.LocalPath).FullName;
            }

            var combinedPath = Path.Combine(fileSystem.GetAbsoluteWorkingDirectory(dashOptions), relativeUri.ToString());

            return fileSystem.DirectoryInfo.FromDirectoryName(combinedPath).FullName;
        }
    }
}
