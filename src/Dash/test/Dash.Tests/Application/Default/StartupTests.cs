// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Dash.Application;
using Dash.Application.Default;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Dash.Tests.Application.Default
{
    public class StartupTests
    {
        [Fact]
        public void CreateServiceCollection_ResolveDashApplication_ShouldResolve()
        {
            // Arrange
            var sut = new Startup();

            // Act
            var result = sut.ConfigureServices(new DashOptions());

            // Assert
            var serviceProvider = result.BuildServiceProvider();
            var service = serviceProvider.GetRequiredService<DashApplication>();
            service.Should().NotBeNull();
        }
    }
}
