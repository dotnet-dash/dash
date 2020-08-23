//using System;
//using System.IO.Abstractions;
//using System.IO.Abstractions.TestingHelpers;
//using System.Net;
//using System.Net.Http;
//using System.Threading.Tasks;
//using Dash.Common.Abstractions;
//using Dash.Engine;
//using Dash.Engine.Abstractions;
//using Dash.Engine.Repositories;
//using Dash.Engine.Visitors;
//using Dash.Nodes;
//using FluentAssertions;
//using NSubstitute;
//using RichardSzalay.MockHttp;
//using Xunit;

//namespace Dash.Tests.Engine.Visitors
//{
//    public class UriResourceDownloadTests
//    {
//        [Fact]
//        public async Task Visit_UriNodeWithUnsupportedScheme_ShouldAddErrorToErrorRepository()
//        {
//            // Arrange
//            var errorRepository = new ErrorRepository();
//            var sut = new UriResourceDownload(Substitute.For<IFileSystem>(), Substitute.For<IConsole>(), errorRepository, Substitute.For<IHttpClientFactory>(), Substitute.For<IClock>());

//            // Act
//            await sut.Visit(new UriNode(new Uri("ftp://ftp.unittest.test")));

//            // Assert
//            errorRepository.GetErrors().Should().SatisfyRespectively(
//                first => first.Should().Be("Unsupported scheme 'ftp' found in 'ftp://ftp.unittest.test/'")
//            );
//        }

//        [Fact]
//        public async Task Visit_UriNodeDownloadError_ShouldAddErrorToErrorRepository()
//        {
//            // Arrange
//            var mockHttpMessageHandler = new MockHttpMessageHandler();
//            mockHttpMessageHandler
//                .Expect(HttpMethod.Get, "https://www.unittest.test")
//                .Respond(HttpStatusCode.InternalServerError);

//            var factory = Substitute.For<IHttpClientFactory>();
//            factory.CreateClient().Returns(mockHttpMessageHandler.ToHttpClient());

//            var errorRepository = new ErrorRepository();

//            var sut = new UriResourceDownload(Substitute.For<IFileSystem>(), Substitute.For<IConsole>(), errorRepository, factory, Substitute.For<IClock>());

//            // Act
//            await sut.Visit(new UriNode(new Uri("https://www.unittest.test")));

//            // Assert
//            errorRepository.GetErrors().Should().SatisfyRespectively(
//                first => first.Should().Be("Error while downloading 'https://www.unittest.test/'. Status code == InternalServerError"));
//        }

//        [Fact]
//        public async Task Visit_UriNodeDownloadSuccess_ShouldHaveSavedLocalCopy()
//        {
//            // Arrange
//            var mockHttpMessageHandler = new MockHttpMessageHandler();
//            mockHttpMessageHandler
//                .Expect(HttpMethod.Get, "https://www.unittest/myfile.csv")
//                .Respond(HttpStatusCode.OK, message => new StringContent("I'm a File"));

//            var factory = Substitute.For<IHttpClientFactory>();
//            factory.CreateClient().Returns(mockHttpMessageHandler.ToHttpClient());

//            var fileSystem = new MockFileSystem();
//            var errorRepository = new ErrorRepository();

//            var clock = Substitute.For<IClock>();
//            clock.UtcNow.Returns(new DateTime(2020, 1, 1, 15, 0, 0));

//            var sut = new UriResourceDownload(fileSystem, Substitute.For<IConsole>(), errorRepository, factory, clock);

//            var node = new UriNode(new Uri("https://www.unittest/myfile.csv"));

//            // Act
//            await sut.Visit(node);

//            // Assert
//            fileSystem.GetFile(node.LocalCopy).TextContents.Should().Be("I'm a File");
//        }
//    }
//}
