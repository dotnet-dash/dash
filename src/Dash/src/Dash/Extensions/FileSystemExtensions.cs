using System.IO.Abstractions;
using System.Linq;

namespace Dash.Extensions
{
    public static class FileSystemExtensions
    {
        public static string? GetNearestEditorConfig(this IFileSystem fileSystem, string path)
        {
            var directoryInfo = fileSystem.DirectoryInfo.FromDirectoryName(path);
            while (directoryInfo != null)
            {
                var files = fileSystem.Directory.GetFiles(directoryInfo.FullName, ".editorconfig");
                if (files.Any())
                {
                    return files.Single();
                }

                var projectFiles = fileSystem.Directory.GetFiles(directoryInfo.FullName, "*.csproj");
                if (projectFiles.Any())
                {
                    return null;
                }

                directoryInfo = directoryInfo.Parent;
            }

            return null;
        }
    }
}
