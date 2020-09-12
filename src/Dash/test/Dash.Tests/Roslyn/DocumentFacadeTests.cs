using Dash.Roslyn.Default;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Dash.Tests.Roslyn
{
    public class DocumentFacadeTests
    {
        [Fact]
        public void Ctor_WithDocument_ShouldMapProperties()
        {
            // Arrange
            var workspace = new AdhocWorkspace();
            var document = workspace.AddProject("Foo", LanguageNames.CSharp).AddDocument("FooBar", "{}");

            // Act
            var sut = new DocumentFacade(document);

            // Assert
            sut.Document.Should().Be(document);
            sut.DocumentId.Should().Be(document.Id);
            sut.FilePath.Should().Be(document.FilePath);
        }
    }
}
