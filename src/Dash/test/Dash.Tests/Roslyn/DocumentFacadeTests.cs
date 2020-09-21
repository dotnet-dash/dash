// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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
