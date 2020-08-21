using System;
using System.IO;
using System.Runtime.Serialization.Json;
using Dash.Exceptions;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Exceptions
{
    public class ParserExceptionTests
    {
        [Fact]
        public void Ctor_Message_ShouldSetProperty()
        {
            // Act
            var sut = new ParserException("Hello, I'm an Exception");

            // Assert
            sut.Message.Should().Be("Hello, I'm an Exception");
        }

        [Fact]
        public void Ctor_MessageAndInnerException_ShouldSetProperties()
        {
            // Act
            var innerException = new Exception("Mr. Exception");
            var sut = new ParserException("Something went horribly wrong", innerException);

            // Assert
            sut.Message.Should().Be("Something went horribly wrong");
            sut.InnerException.Should().Be(innerException);
        }

        [Fact]
        public void Ctor_SerializationInfo_ShouldDeserialize()
        {
            // Arrange
            var sut = new ParserException("I'm an exceptional Exception");

            var stream = new MemoryStream();
            var serializer = new DataContractJsonSerializer(typeof(ParserException));
            serializer.WriteObject(stream, sut);
            stream.Position = 0;

            // Act
            var deserialized = serializer.ReadObject(stream);

            // Assert
            deserialized.Should().BeOfType<ParserException>().Subject.Message.Should().Be("I'm an exceptional Exception");
        }
    }
}
