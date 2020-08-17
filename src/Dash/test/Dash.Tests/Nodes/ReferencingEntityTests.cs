//using Dash.Nodes;
//using FluentAssertions;
//using Xunit;

//namespace Dash.Tests.Nodes
//{
//    public class ReferencingEntityTests
//    {
//        private readonly Entity _entity;

//        public ReferencingEntityTests()
//        {
//            _entity = new Entity("MyEntity");
//        }

//        [Fact]
//        public void Ctor_Entity_NameShouldBeEntityName()
//        {
//            // Act
//            var sut = new ReferencingEntity(_entity);

//            // Assert
//            sut.Name.Should().Be(_entity.Name);
//        }

//        [Fact]
//        public void Ctor_Entity_EntityShouldBeReferenced()
//        {
//            // Act
//            var sut = new ReferencingEntity(_entity);

//            // Assert
//            sut.Entity.Should().Be(_entity);
//        }

//        [Fact]
//        public void Ctor_Entity_IsNullableShouldBeFalse()
//        {
//            // Act
//            var sut = new ReferencingEntity(_entity);

//            // Assert
//            sut.IsNullable.Should().BeFalse();
//        }

//        [Fact]
//        public void Ctor_NameSpecified_ShouldUseName()
//        {
//            // Act
//            var sut = new ReferencingEntity("CustomName", _entity);

//            // Assert
//            sut.Name.Should().Be("CustomName");
//        }

//        [Fact]
//        public void Ctor_IsNullableSpecified_IsNullableShouldBeTrue()
//        {
//            // Act
//            var sut = new ReferencingEntity("CustomName", _entity, true);

//            // Assert
//            sut.IsNullable.Should().BeTrue();
//        }
//    }
//}
