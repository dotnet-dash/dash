using System.IO;
using System.Runtime.Serialization.Json;
using Dash.Exceptions;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Exceptions
{
    public class EntityModelNotFoundExceptionTests
    {
        [Fact]
        public void Ctor_Message_ShouldSetProperty()
        {
            // Act
            var sut = new EntityModelNotFoundException("Hello, I'm an Exception");

            // Assert
            sut.Message.Should().Be("Hello, I'm an Exception");
        }

        [Fact]
        public void Ctor_SerializationInfo_ShouldDeserialize()
        {
            // Arrange
            var sut = new EntityModelNotFoundException("I'm an exceptional Exception");

            var stream = new MemoryStream();
            var serializer = new DataContractJsonSerializer(typeof(EntityModelNotFoundException));
            serializer.WriteObject(stream, sut);
            stream.Position = 0;

            // Act
            var deserialized = serializer.ReadObject(stream);

            // Assert
            deserialized.Should().BeOfType<EntityModelNotFoundException>().Subject.Message.Should().Be("I'm an exceptional Exception");
        }
    }

    public class InvalidDataTypeExceptionTests
    {
        [Fact]
        public void Ctor_SpecifiedDashDataType_ShouldSetMessageWithSpecifiedDashDataType()
        {
            // Act
            var sut = new InvalidDataTypeException("Unknown");

            // Assert
            sut.Message.Should().Be("The specified datatype 'Unknown' is invalid");
        }

        [Fact]
        public void Ctor_SerializationInfo_ShouldDeserialize()
        {
            // Arrange
            var sut = new InvalidDataTypeException("varchar");

            var stream = new MemoryStream();
            var serializer = new DataContractJsonSerializer(typeof(InvalidDataTypeException));
            serializer.WriteObject(stream, sut);
            stream.Position = 0;

            // Act
            var deserialized = serializer.ReadObject(stream);

            // Assert
            deserialized.Should().BeOfType<InvalidDataTypeException>().Subject.Message.Should().Be("The specified datatype 'varchar' is invalid");
        }
    }
}