// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using Dash.Engine.TemplateTransformers.Scriban;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Engine.TemplateTransformers.Scriban
{
    public class GetCSharpLiteralFormatterTests
    {
        [Theory]
        [MemberData(nameof(Data), 2)]
        public void GetCSharpLiteral_Null_ShouldReturnValueNull(object value, string expectedOutput)
        {
            // Act
            var result = GetCSharpLiteralFormatter.GetCSharpLiteral(value);

            // Assert
            result.Should().Be(expectedOutput);
        }

        public static IEnumerable<object?[]> Data =>
            new List<object?[]>
            {
                new object?[] { null, "null" },
                new object?[] { 1234, "1234" },
                new object?[] { 12.34m, "12.34m"},
                new object?[] { "", "\"\"" },
                new object?[] { "\"", "\"\\\"\"" },
                new object?[] { "foo", "\"foo\"" },
            };
    }
}
