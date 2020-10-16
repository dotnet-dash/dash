// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Dash.Common;
using Dash.Engine.Repositories;
using FluentArrange.NSubstitute;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Engine.Repositories
{
    public class UriResourceRepositoryTests
    {
        [Theory]
        [InlineData("dash://embedded", "dash://embedded/")]
        [InlineData("file://c:/foo/bar.csv", "file:///c:/foo/bar.csv")]
        public async Task Add_UriResourceNotAdded_ShouldAdd(string uri, string expectedResult)
        {
            // Arrange
            var context = Arrange.For<UriResourceRepository>()
                .WithDependency<IFileSystem>(new MockFileSystem());

            // Act
            await context.Sut.Add(new Uri(uri));

            // Assert
            (await context.Sut.Count()).Should().Be(1);
            (await context.Sut.Get(new Uri(uri))).Should().Be(expectedResult);
        }

        [Fact]
        public async Task Add_UriResourceAlreadyAdded_ShouldNotAdd()
        {
            // Arrange
            var context = Arrange.For<UriResourceRepository>()
                .WithDependency<IFileSystem>(new MockFileSystem());

            await context.Sut.Add(new Uri("file://c:/foo/bar.csv"));

            // Act
            await context.Sut.Add(new Uri("file://c:/foo/bar.csv"));

            // Assert
            (await context.Sut.Count()).Should().Be(1);
        }

        [Fact]
        public async Task Add_UriFileNameContents_ShouldAddAndWriteToFile()
        {
            // Arrange
            var context = Arrange.For<UriResourceRepository>()
                .WithDependency<ISessionService>(d => d.GetTempPath("bar.csv").Returns("c:/temp/bar.csv"))
                .WithDependency<IFileSystem>(new MockFileSystem());

            var bytes = new byte[] { 1 };

            // Act
            await context.Sut.Add(new Uri("https://foo/bar.csv"), "bar.csv", bytes);

            // Assert
            context.Dependency<IFileSystem>().File.Exists("c:/temp/bar.csv").Should().BeTrue();
        }

        [Fact]
        public void Get_UriResourceDoesNotExist_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var sut = Arrange.Sut<UriResourceRepository>();

            // Act
            Action act = () => sut.Get(new Uri("file:///c:/foo/bar.csv"));

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Uri 'file:///c:/foo/bar.csv' not in repository");
        }

        [Fact]
        public async Task Get_UriResourceDoesExist_ShouldReturnValue()
        {
            // Arrange
            var sut = Arrange.Sut<UriResourceRepository>(async s =>
            {
                await s.Add(new Uri("file:///foo.csv"));
            });

            // Act
            var result = await sut.Get(new Uri("file:///foo.csv"));

            // Assert
            result.Should().NotBeNull();
        }
    }
}