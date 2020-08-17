using System;
using Dash.Engine.LanguageProviders;
using Dash.Exceptions;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Engine.LanguageProviders
{
    public class BaseLanguageProviderTests
    {
        [Fact]
        public void Translate_UnknownDataType_ShouldThrowInvalidDataTypeException()
        {
            // Arrange
            var sut = Substitute.For<BaseLanguageProvider>();

            // Act
            Action act = () => sut.Translate("unknown");

            // Assert
            act.Should().Throw<InvalidDataTypeException>()
                .And.Message.Should().Be("The specified datatype 'unknown' is invalid");
        }
    }
}
