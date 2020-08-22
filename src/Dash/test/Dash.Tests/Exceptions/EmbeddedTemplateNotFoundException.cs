using System.IO;
using System.Runtime.Serialization.Json;
using Dash.Exceptions;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Exceptions
{
    public class EmbeddedTemplateNotFoundExceptionTests
    {
        [Fact]
        public void Ctor_Message_ShouldSetProperty()
        {
            // Act
            var sut = new EmbeddedTemplateNotFoundException("Hello, I'm an Exception");

            // Assert
            sut.Message.Should().Be("Hello, I'm an Exception");
        }

        [Fact]
        public void Ctor_SerializationInfo_ShouldDeserialize()
        {
            // Arrange
            var sut = new EmbeddedTemplateNotFoundException("I'm an exceptional Exception");

            var stream = new MemoryStream();
            var serializer = new DataContractJsonSerializer(typeof(EmbeddedTemplateNotFoundException));
            serializer.WriteObject(stream, sut);
            stream.Position = 0;

            // Act
            var deserialized = serializer.ReadObject(stream);

            // Assert
            deserialized.Should().BeOfType<EmbeddedTemplateNotFoundException>().Subject.Message.Should().Be("I'm an exceptional Exception");
        }
    }
}
