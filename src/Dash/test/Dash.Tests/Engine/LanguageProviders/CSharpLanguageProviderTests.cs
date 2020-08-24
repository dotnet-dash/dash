// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Dash.Engine.LanguageProviders;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Engine.LanguageProviders
{
    public class CSharpLanguageProviderTests
    {
        [Theory]
        [InlineData("Int", "int")]
        [InlineData("Bool", "bool")]
        [InlineData("DateTime", "System.DateTime")]
        [InlineData("Email", "string")]
        [InlineData("Guid", "System.Guid")]
        [InlineData("String", "string")]
        [InlineData("Unicode", "string")]
        public void Translate_GivenDashDataType_ShouldReturnExpectedDataType(string dashDataType, string expectedDataType)
        {
            // Arrange
            var sut = new CSharpLanguageProvider();

            // Act
            var result = sut.Translate(dashDataType);

            // Assert
            result.Should().Be(expectedDataType);
        }
    }
}
