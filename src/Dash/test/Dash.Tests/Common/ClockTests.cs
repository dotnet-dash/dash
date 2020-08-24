// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using Dash.Common.Default;
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
