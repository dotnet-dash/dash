// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Threading.Tasks;
using Dash.Common;
using Dash.Engine;
using Dash.Engine.Visitors;
using Dash.Nodes;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Engine.Visitors
{
    public class UriResourceDownloadTests
    {
        private readonly UriResourceDownload _sut;
        private readonly IUriResourceRepository _uriResourceRepository = Substitute.For<IUriResourceRepository>();
        private readonly IHttpUriDownloader _httpUriDownloader = Substitute.For<IHttpUriDownloader>();

        public UriResourceDownloadTests()
        {
            _sut = new UriResourceDownload(Substitute.For<IConsole>(),
                _uriResourceRepository,
                _httpUriDownloader);
        }

        [Fact]
        public async Task Visit_UriNode_ResourceAlreadyExists_ShouldNotCallRepository()
        {
            // Arrange
            _uriResourceRepository.Exists(new Uri("file://localhost/foo/bar.csv")).Returns(true);

            var node = new UriNode(new Uri("file://localhost/foo/bar.csv"), true);

            // Act
            await _sut.Visit(node);

            // Assert
            await _uriResourceRepository.Received(0).Add(Arg.Any<Uri>());
            await _uriResourceRepository.Received(0).Add(Arg.Any<Uri>(), Arg.Any<string>(), Arg.Any<byte[]>());
        }

        [Fact]
        public async Task Visit_UriNode_HttpResourceNotFoundInRepository_ShouldDownload()
        {
            // Arrange
            var node = new UriNode(new Uri("https://foo/bar.csv"), true);

            // Act
            await _sut.Visit(node);

            // Assert
            await _httpUriDownloader.Received(1).Download(new Uri("https://foo/bar.csv"));
        }

        [Fact]
        public async Task Visit_UriNode_IsFile_ShouldNotDownload()
        {
            // Arrange
            var node = new UriNode(new Uri("file://localhost/foo/bar.csv"), true);

            // Act
            await _sut.Visit(node);

            // Assert
            await _httpUriDownloader.Received(0).Download(Arg.Any<Uri>());
        }

        [Fact]
        public async Task Visit_UriNode_IsFile_ShouldAddToRepository()
        {
            // Arrange
            var node = new UriNode(new Uri("file://localhost/foo/bar.csv"), true);

            // Act
            await _sut.Visit(node);

            // Assert
            await _uriResourceRepository.Received(1).Add(new Uri("file://localhost/foo/bar.csv"));
        }

        [Theory]
        [InlineData("http://foo/bar.csv")]
        [InlineData("https://foo/bar.csv")]
        public async Task Visit_UriNode_HttpResourceDownloadSuccess_ShouldAddToRepository(string uri)
        {
            // Arrange
            var content = new byte[] { };
            _httpUriDownloader.Download(new Uri(uri)).Returns((true, "bar.csv", content));

            var node = new UriNode(new Uri(uri), true);

            // Act
            await _sut.Visit(node);

            // Assert
            await _uriResourceRepository.Received(1).Add(new Uri(uri), "bar.csv", content);
        }

        [Fact]
        public async Task Visit_UriNode_HttpResourceDownloadFailure_ShouldNotAddToRepository()
        {
            // Arrange
            _httpUriDownloader.Download(new Uri("https://foo/bar.csv")).Returns((false, null, null));

            var node = new UriNode(new Uri("https://foo/bar.csv"), true);

            // Act
            await _sut.Visit(node);

            // Assert
            await _uriResourceRepository.Received(0).Add(Arg.Any<Uri>(), Arg.Any<string>(), Arg.Any<byte[]>());
        }
    }
}
