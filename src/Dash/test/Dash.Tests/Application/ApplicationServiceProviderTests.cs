using Dash.Application;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Dash.Tests.Application
{
    public class ApplicationServiceProviderTests
    {
        [Fact]
        public void Create_ResolveDashApplication_ShouldResolve()
        {
            // Arrange
            var sut = new ApplicationServiceProvider();

            // Act
            var result = sut.Create(true, null);

            // Assert
            var service = result.GetRequiredService(typeof(DashApplication));
            service.Should().NotBeNull();
        }
    }
}
