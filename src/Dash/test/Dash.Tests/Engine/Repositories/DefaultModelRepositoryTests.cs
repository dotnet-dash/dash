using System;
using Dash.Engine.Models;
using Dash.Engine.Repositories;
using Dash.Exceptions;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Engine.Repositories
{
    public class DefaultModelRepositoryTests
    {
        [Fact]
        public void Add_EntityModelWithExistingName_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var sut = new DefaultModelRepository();
            sut.Add(new EntityModel("Account"));

            // Act
            Action act = () => sut.Add(new EntityModel("Account"));

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("Cannot add a duplicate EntityModel");
        }

        [Fact]
        public void Get_EntityNameDoesNotExist_ShouldThrowInvalidOperation()
        {
            // Arrange
            var sut = new DefaultModelRepository();

            // Act
            Action act = () => sut.Get("Order");

            // Assert
            act.Should().Throw<EntityModelNotFoundException>();
        }
    }
}
