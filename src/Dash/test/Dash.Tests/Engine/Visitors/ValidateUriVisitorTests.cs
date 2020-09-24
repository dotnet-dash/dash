// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Dash.Common;
using Dash.Engine;
using Dash.Engine.Repositories;
using Dash.Engine.Visitors;
using Dash.Nodes;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Engine.Visitors
{
    public class ValidateUriVisitorTests
    {
        private readonly ErrorRepository _errorRepository = new ErrorRepository();
        private readonly MockFileSystem _mockFileSystem = new MockFileSystem();
        private readonly ValidateUriExistsVisitor _sut;

        public ValidateUriVisitorTests()
        {
            _sut = new ValidateUriExistsVisitor(Substitute.For<IConsole>(),
                _mockFileSystem,
                _errorRepository,
                Substitute.For<IEmbeddedTemplateProvider>());
        }

        [Fact]
        public async Task Visit_UriNode_ExistingFileExpectedButNotFound_ShouldAddErrorToRepository()
        {
            // Arrange
            var uriNode = UriNode.ForExistingFile(new Uri("file:///c:/foo.txt"));

            // Act
            await _sut.Visit(uriNode);

            // Assert
            _errorRepository.GetErrors().Should().SatisfyRespectively(first => first.Should().Be("File does not exist: 'file:///c:/foo.txt'"));
        }

        [Fact]
        public async Task Visit_UriNode_ExistingFileExpectedFound_ShouldNotAddErrorToRepository()
        {
            // Arrange
            _mockFileSystem.AddFile("c:/foo.txt", new MockFileData("Foo"));

            var uriNode = UriNode.ForExistingFile(new Uri("file:///c:/foo.txt"));

            // Act
            await _sut.Visit(uriNode);

            // Assert
            _errorRepository.GetErrors().Should().BeEmpty();
        }

        [Fact]
        public async Task Visit_UriNode_FileNotExpectedToExist_ShouldNotAddErrorToRepository()
        {
            // Arrange
            var uriNode = UriNode.ForFileOutput(new Uri("file:///c:/foo.txt"));

            // Act
            await _sut.Visit(uriNode);

            // Assert
            _errorRepository.GetErrors().Should().BeEmpty();
        }

        [Fact]
        public async Task Visit_UriNode_UnsupportedSchemeFound_ShouldNotAddErrorToRepository()
        {
            // Arrange
            var uriNode = UriNode.ForFileOutput(new Uri("https://foo"));

            // Act
            await _sut.Visit(uriNode);

            // Assert
            _errorRepository.GetErrors().Should().BeEmpty();
        }
    }
}