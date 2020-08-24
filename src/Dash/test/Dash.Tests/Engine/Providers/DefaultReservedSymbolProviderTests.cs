// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Dash.Engine.Providers;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Engine.Providers
{
    public class DefaultReservedSymbolProviderTests
    {
        [Theory]
        [InlineData("base", false)]
        [InlineData("Base", false)]
        [InlineData("object", true)]
        [InlineData("hello", false)]
        [InlineData("world", false)]
        public void IsReservedEntityName_GivenTheory_ShouldSatisfyExpectedResult(string keyword, bool expectedResult)
        {
            // Arrange
            var sut = new DefaultReservedSymbolProvider();

            // Act
            var result = sut.IsReservedEntityName(keyword);

            // Assert
            result.Should().Be(expectedResult);
        }
    }
}
