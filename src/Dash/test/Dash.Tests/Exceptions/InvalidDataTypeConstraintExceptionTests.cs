using System.IO;
using System.Runtime.Serialization.Json;
using Dash.Exceptions;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Exceptions
{
    public class InvalidDataTypeConstraintExceptionTests
    {
        [Fact]
        public void Ctor_Message_ShouldSetProperty()
        {
            // Act
            var sut = new InvalidDataTypeConstraintException("Hello, I'm an Exception");

            // Assert
            sut.Message.Should().Be("Hello, I'm an Exception");
        }

        [Fact]
        public void Ctor_SerializationInfo_ShouldDeserialize()
        {
            // Arrange
            var sut = new InvalidDataTypeConstraintException("I'm an exceptional Exception");

            var stream = new MemoryStream();
            var serializer = new DataContractJsonSerializer(typeof(InvalidDataTypeConstraintException));
            serializer.WriteObject(stream, sut);
            stream.Position = 0;

            // Act
            var deserialized = serializer.ReadObject(stream);

            // Assert
            deserialized.Should().BeOfType<InvalidDataTypeConstraintException>().Subject.Message.Should().Be("I'm an exceptional Exception");
        }
    }
}