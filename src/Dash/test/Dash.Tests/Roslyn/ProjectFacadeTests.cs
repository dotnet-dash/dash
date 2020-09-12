using Dash.Roslyn.Default;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Dash.Tests.Roslyn
{
    public class ProjectFacadeTests
    {
        [Fact]
        public void AddDocument_WithPathAndText_ShouldReturnAddedDocument()
        {
            // Arrange
            var workspace = new AdhocWorkspace();
            var project = workspace.AddProject("Foo", LanguageNames.CSharp);

            var sut = new ProjectFacade(project);

            // Act
            var document = sut.AddDocument("Foo", "Bar");

            // Assert
            document.FilePath.Should().Be("Foo");
            document.Document.Name.Should().Be("Foo");
        }

        [Fact]
        public void GetDocument_MultipleDocumentsAdded_ShouldReturnCorrectDocument()
        {
            // Arrange
            var workspace = new AdhocWorkspace();
            var project = workspace.AddProject("Foo", LanguageNames.CSharp);

            var sut = new ProjectFacade(project);

            sut.AddDocument("Foo", "Foo");
            var expectedDocument = sut.AddDocument("FooBar", "FooBar");
            sut.AddDocument("Foo Bar", "Foo Bar");

            // Act
            var result = sut.GetDocument(expectedDocument.DocumentId);

            // Assert
            result.DocumentId.Should().Be(expectedDocument.DocumentId);
            result.Document.Name.Should().Be(expectedDocument.Document.Name);
            result.FilePath.Should().Be(expectedDocument.FilePath);
        }
    }
}
