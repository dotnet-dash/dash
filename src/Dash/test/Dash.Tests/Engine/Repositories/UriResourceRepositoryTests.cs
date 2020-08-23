using System;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Dash.Common.Abstractions;
using Dash.Engine.Abstractions;
using Dash.Engine.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Engine.Repositories
{
    public class UriResourceRepositoryTests
    {
        private readonly IFileSystem _fileSystem = new MockFileSystem();
        private readonly ISessionService _sessionService = Substitute.For<ISessionService>();
        private readonly UriResourceRepository _sut;

        public UriResourceRepositoryTests()
        {
            _sut = new UriResourceRepository(
                Substitute.For<IConsole>(),
                _fileSystem,
                _sessionService,
                Substitute.For<IEmbeddedTemplateProvider>());
        }

        [Theory]
        [InlineData("dash://embedded", "dash://embedded/")]
        [InlineData("file://c:/foo/bar.csv", "file:///c:/foo/bar.csv")]
        public async Task Add_UriResourceNotAdded_ShouldAdd(string uri, string expectedResult)
        {
            // Act
            await _sut.Add(new Uri(uri));

            // Assert
            var count = await _sut.Count();
            count.Should().Be(1);

            var result = await _sut.Get(new Uri(uri));
            result.Should().Be(expectedResult);
        }

        [Fact]
        public async Task Add_UriResourceAlreadyAdded_ShouldNotAdd()
        {
            // Arrange
            await _sut.Add(new Uri("file://c:/foo/bar.csv"));

            // Act
            await _sut.Add(new Uri("file://c:/foo/bar.csv"));

            // Assert
            var count = await _sut.Count();
            count.Should().Be(1);
        }

        [Fact]
        public async Task Add_UriFileNameContents_ShouldAddAndWriteToFile()
        {
            // Arrange
            var bytes = new byte[] {1};
            _sessionService.GetTempPath("bar.csv").Returns("c:/temp/bar.csv");

            // Act
            await _sut.Add(new Uri("https://foo/bar.csv"), "bar.csv", bytes);

            // Assert
            _fileSystem.File.Exists("c:/temp/bar.csv").Should().BeTrue();
        }

        [Fact]
        public void Get_UriResourceDoesNotExist_ShouldThrowInvalidOperationException()
        {
            // Act
            Action act = () => _sut.Get(new Uri("file:///c:/foo/bar.csv"));

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Uri 'file:///c:/foo/bar.csv' not in repository");
        }

        [Fact]
        public async Task Get_UriResourceDoesExist_ShouldReturnValue()
        {
            // Arrange
            await _sut.Add(new Uri("file:///foo.csv"));

            // Act
            var result = await _sut.Get(new Uri("file:///foo.csv"));

            // Assert
            result.Should().NotBeNull();
        }
    }
}