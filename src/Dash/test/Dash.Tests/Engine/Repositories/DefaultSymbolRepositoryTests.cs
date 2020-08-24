// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Dash.Engine.Repositories;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Engine.Repositories
{
    public class DefaultSymbolRepositoryTests
    {
        [Fact]
        public void AddEntity_EntityNameDoesNotExist_ShouldAdd()
        {
            // Arrange
            var sut = new DefaultSymbolRepository();

            // Act
            sut.AddEntity("Foo");

            // Assert
            sut.GetEntityNames().Should().SatisfyRespectively(first => first.Should().Be("Foo"));
        }

        [Fact]
        public void EntityExists_EntityAdded_ShouldReturnTrue()
        {
            // Arrange
            var sut = new DefaultSymbolRepository();
            sut.AddEntity("Foo");

            // Act
            var result = sut.EntityExists("Foo");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void EntityExists_EntityNotAdded_ShouldReturnFalse()
        {
            // Arrange
            var sut = new DefaultSymbolRepository();

            // Act
            var result = sut.EntityExists("Foo");

            // Assert
            result.Should().BeFalse();
        }
    }
}
