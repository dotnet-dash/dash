// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Dash.Engine;
using Dash.Engine.Visitors;
using Dash.Nodes;
using NSubstitute;
using Xunit;
using Arrange = FluentArrange.NSubstitute.Arrange;

namespace Dash.Tests.Engine.Visitors
{
    public class ValidateUriExistsVisitorTests
    {
        [Fact]
        public async Task Visit_UriNode_ExistingFileExpectedButNotFound_ShouldAddErrorToRepository()
        {
            // Arrange
            var context = Arrange.For<ValidateUriExistsVisitor>();

            var uriNode = UriNode.ForExistingFile(new Uri("file:///c:/foo.txt"));

            // Act
            await context.Sut.Visit(uriNode);

            // Assert
            context.Dependency<IErrorRepository>().Received(1).Add("File does not exist: 'file:///c:/foo.txt'");
        }

        [Fact]
        public async Task Visit_UriNode_ExistingFileExpectedFound_ShouldNotAddErrorToRepository()
        {
            // Arrange
            var context = Arrange.For<ValidateUriExistsVisitor>()
                .WithDependency<IFileSystem, MockFileSystem>(new MockFileSystem(), fileSystem =>
                {
                    fileSystem.AddFile("c:/foo.txt", new MockFileData("Foo"));
                });

            var uriNode = UriNode.ForExistingFile(new Uri("file:///c:/foo.txt"));

            // Act
            await context.Sut.Visit(uriNode);

            // Assert
            context.Dependency<IErrorRepository>().DidNotReceive().Add(Arg.Any<string>());
        }

        [Fact]
        public async Task Visit_UriNode_FileNotExpectedToExist_ShouldNotAddErrorToRepository()
        {
            // Arrange
            var context = Arrange.For<ValidateUriExistsVisitor>();

            var uriNode = UriNode.ForOutputFile(new Uri("file:///c:/foo.txt"));

            // Act
            await context.Sut.Visit(uriNode);

            // Assert
            context.Dependency<IErrorRepository>().DidNotReceive().Add(Arg.Any<string>());
        }

        [Fact]
        public async Task Visit_UriNode_UnsupportedSchemeFound_ShouldNotAddErrorToRepository()
        {
            // Arrange
            var context = Arrange.For<ValidateUriExistsVisitor>();

            var uriNode = UriNode.ForOutputFile(new Uri("https://foo"));

            // Act
            await context.Sut.Visit(uriNode);

            // Assert
            context.Dependency<IErrorRepository>().DidNotReceive().Add(Arg.Any<string>());
        }
    }
}