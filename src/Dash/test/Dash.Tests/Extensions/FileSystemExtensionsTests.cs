using System.IO.Abstractions.TestingHelpers;
using Dash.Extensions;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Extensions
{
    public class FileSystemExtensionsTests
    {
        private readonly MockFileData _foo = MockFileData.NullObject;

        [Fact]
        public void GetNearestEditorConfig_NoEditorConfigInCurrentDirectory_ShouldReturnNull()
        {
            // Arrange
            var sut = new MockFileSystem();

            // Act
            var result = sut.GetNearestEditorConfig("c:/temp");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetNearestEditorConfig_EditorConfigFoundInCurrentDirectory_ShouldReturnEditorConfigPath()
        {
            // Arrange
            var sut = new MockFileSystem();
            sut.AddFile("c:/temp/.editorconfig", _foo);

            // Act
            var result = sut.GetNearestEditorConfig("c:/temp");

            // Assert
            result.Should().Be(@"C:\temp\.editorconfig");
        }

        [Fact]
        public void GetNearestEditorConfig_NoEditorConfigFoundInAncestorDirectories_ShouldReturnNull()
        {
            // Arrange
            var sut = new MockFileSystem();
            sut.AddFile("c:/temp/.editorconfig", _foo);
            sut.AddFile("c:/temp/project/myproject.csproj", _foo);
            sut.AddFile("c:/temp/project/controllers/foo.cs", _foo);

            // Act
            var result = sut.GetNearestEditorConfig("c:/temp/project/controllers");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetNearestEditorConfig_EditorConfigFoundInParentDirectory_ShouldReturnEditorConfigPath()
        {
            // Arrange
            var sut = new MockFileSystem();
            sut.AddFile("c:/temp/project/.editorconfig", _foo);
            sut.AddFile("c:/temp/project/controllers/foo.cs", _foo);

            // Act
            var result = sut.GetNearestEditorConfig("c:/temp/project/controllers");

            // Assert
            result.Should().Be(@"C:\temp\project\.editorconfig");
        }

        [Fact]
        public void GetNearestEditorConfig_MultipleEditorConfigInProjectDirectory_ShouldReturnNearest()
        {
            // Arrange
            var sut = new MockFileSystem();
            sut.AddFile("c:/temp/project/.editorconfig", _foo);
            sut.AddFile("c:/temp/project/controllers/foo.cs", _foo);
            sut.AddFile("c:/temp/project/controllers/.editorconfig", _foo);

            // Act
            var result = sut.GetNearestEditorConfig("c:/temp/project/controllers");

            // Assert
            result.Should().Be(@"C:\temp\project\controllers\.editorconfig");
        }
    }
}
