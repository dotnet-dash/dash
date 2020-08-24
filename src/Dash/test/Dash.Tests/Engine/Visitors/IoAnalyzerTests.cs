// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Dash.Common;
using Dash.Engine.Repositories;
using Dash.Engine.Visitors;
using Dash.Nodes;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Engine.Visitors
{
    public class IoAnalyzerTests
    {
        private readonly ErrorRepository _errorRepository = new ErrorRepository();
        private readonly MockFileSystem _mockFileSystem = new MockFileSystem();
        private readonly IoAnalyzer _sut;

        public IoAnalyzerTests()
        {
            _sut = new IoAnalyzer(NSubstitute.Substitute.For<IConsole>(),
                _mockFileSystem,
                _errorRepository);
        }

        [Fact]
        public async Task Visit_UriNode_UriMustExistAndFileDoesNotExist_ShouldAddErrorToRepository()
        {
            // Arrange
            var uriNode = new UriNode(new Uri("file:///c:/foo.txt"), true);

            // Act
            await _sut.Visit(uriNode);

            // Assert
            _errorRepository.GetErrors().Should().SatisfyRespectively(
                first => first.Should().Be("File does not exist: 'file:///c:/foo.txt'"));
        }

        [Fact]
        public async Task Visit_UriNode_UriMustExistAndFileExists_ShouldNotAddErrorToRepository()
        {
            // Arrange
            _mockFileSystem.AddFile("c:/foo.txt", new MockFileData("Foo"));

            var uriNode = new UriNode(new Uri("file:///c:/foo.txt"), true);

            // Act
            await _sut.Visit(uriNode);

            // Assert
            _errorRepository.GetErrors().Should().BeEmpty();
        }

        [Fact]
        public async Task Visit_UriNode_UriDoesNotHaveToExistAndFileNotExist_ShouldNotAddErrorToRepository()
        {
            // Arrange
            var uriNode = new UriNode(new Uri("file:///c:/foo.txt"), false);

            // Act
            await _sut.Visit(uriNode);

            // Assert
            _errorRepository.GetErrors().Should().BeEmpty();
        }
    }
}