using System;
using Dash.Common;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Common
{
    public class ClockTests
    {
        [Fact]
        public void UtcNow_Get_ShouldBeCloseToSystemDateTime()
        {
            // Arrange
            var sut = new Clock();
            var now = DateTime.UtcNow;

            // Act
            var utcNow = sut.UtcNow;

            // Assert
            utcNow.Should().BeCloseTo(now, 500);
        }
    }
}
