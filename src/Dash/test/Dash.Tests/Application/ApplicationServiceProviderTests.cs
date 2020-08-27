// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Dash.Application;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Dash.Tests.Application
{
    public class ApplicationServiceProviderTests
    {
        [Fact]
        public void CreateServiceCollection_ResolveDashApplication_ShouldResolve()
        {
            // Arrange
            var sut = new ApplicationServiceProvider();

            // Act
            var result = sut.CreateServiceCollection(true, ".");

            // Assert
            var serviceProvider = result.BuildServiceProvider();
            var service = serviceProvider.GetRequiredService(typeof(DashApplication));
            service.Should().NotBeNull();
        }
    }
}
