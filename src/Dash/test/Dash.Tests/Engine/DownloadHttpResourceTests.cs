using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Dash.Common;
using Dash.Common.Default;
using Dash.Engine;
using Dash.Engine.Repositories;
using FluentAssertions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Xunit;

namespace Dash.Tests.Engine
{
    public class DownloadHttpResourceTests
    {
        private readonly IHttpClientFactory _factory = Substitute.For<IHttpClientFactory>();
        private readonly IErrorRepository _errorRepository = new ErrorRepository();
        private readonly HttpUriDownloader _sut;

        public DownloadHttpResourceTests()
        {
            _sut = new HttpUriDownloader(Substitute.For<IConsole>(), _factory, _errorRepository);
        }

        [Theory]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.BadGateway)]
        public async Task Download_InternalServerError_ShouldHaveAddedErrorToRepository(HttpStatusCode httpStatusCode)
        {
            // Arrange
            var mockHttpMessageHandler = new MockHttpMessageHandler();
            mockHttpMessageHandler
                .Expect(HttpMethod.Get, "https://foo/bar.csv")
                .Respond(httpStatusCode);

            _factory.CreateClient().Returns(mockHttpMessageHandler.ToHttpClient());

            // Act
            await _sut.Download(new Uri("https://foo/bar.csv"));

            // Assert
            _errorRepository.GetErrors().Should().SatisfyRespectively(
                first => first.Should().Be($"Error while downloading 'https://foo/bar.csv'. Status code == {httpStatusCode}"));
        }

        [Fact]
        public async Task Download_UriNotFound_ShouldReturnFailedTuple()
        {
            // Arrange
            var mockHttpMessageHandler = new MockHttpMessageHandler();
            _factory.CreateClient().Returns(mockHttpMessageHandler.ToHttpClient());

            // Act
            var result = await _sut.Download(new Uri("https://foo/bar.csv"));

            // Assert
            result.Should().Be((false, null, null));
        }

        [Fact]
        public async Task Visit_UriNodeDownloadSuccess_ShouldReturnSuccessfulTuple()
        {
            // Arrange
            var stringContent = new StringContent("I'm a File");

            var mockHttpMessageHandler = new MockHttpMessageHandler();
            mockHttpMessageHandler
                .Expect(HttpMethod.Get, "https://foo/bar.csv")
                .Respond(HttpStatusCode.OK, message => stringContent);

            _factory.CreateClient().Returns(mockHttpMessageHandler.ToHttpClient());

            var clock = Substitute.For<IClock>();
            clock.UtcNow.Returns(new DateTime(2020, 1, 1, 15, 0, 0));

            // Act
            var result = await _sut.Download(new Uri("https://foo/bar.csv"));

            // Assert
            result.Success.Should().BeTrue();
            result.FileName.Should().Be("bar.csv");
            result.Content.SequenceEqual(await stringContent.ReadAsByteArrayAsync()).Should().BeTrue();
        }
    }
}
