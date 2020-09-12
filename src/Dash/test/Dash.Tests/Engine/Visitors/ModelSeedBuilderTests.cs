// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Text;
using System.Threading.Tasks;
using Dash.Common;
using Dash.Engine;
using Dash.Engine.Models;
using Dash.Engine.Repositories;
using Dash.Engine.Visitors;
using Dash.Nodes;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Engine.Visitors
{
    public class ModelSeedBuilderTests
    {
        private readonly MockFileSystem _mockFileSystem = new MockFileSystem();
        private readonly IUriResourceRepository _uriResourceRepository = Substitute.For<IUriResourceRepository>();
        private readonly IModelRepository _modelRepository = new DefaultModelRepository();
        private readonly IErrorRepository _errorRepository = new ErrorRepository();
        private readonly ModelSeedBuilder _sut;

        public ModelSeedBuilderTests()
        {
            var csvContent = new StringBuilder();
            csvContent.AppendLine("Code;Name;NumericCode");
            csvContent.AppendLine("EUR;Euro;978");
            csvContent.AppendLine("USD;US Dollars;840");
            _mockFileSystem.AddFile("c:\\currencies.csv", new MockFileData(csvContent.ToString()));

            _uriResourceRepository.Get(new Uri("https://currencycode")).Returns("c:\\currencies.csv");

            _sut = new ModelSeedBuilder(Substitute.For<IConsole>(),
                _mockFileSystem,
                _modelRepository,
                _errorRepository,
                _uriResourceRepository);
        }

        [Fact]
        public async Task Visit_CsvSeedDeclarationNode_ShouldHaveAddedSeedDataToEntity()
        {
            // Arrange
            var entityModel = ArrangeEntityModel();
            _modelRepository.Add(entityModel);

            var node = new CsvSeedDeclarationNode(new EntityDeclarationNode(new ModelNode(), entityModel.Name),
                new Uri("https://currencycode"), true, ";",
                new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    {"CurrencyCode", "Code"},
                    {"CurrencyName", "Name"},
                    {"NumericCode", "NumericCode"}
                });

            // Act
            await _sut.Visit(node);

            // Assert
            _errorRepository.HasErrors().Should().BeFalse();

            _modelRepository
                .Get("Currency")
                .SeedData.Should().SatisfyRespectively(
                    first =>
                    {
                        first.Keys.Should().SatisfyRespectively(
                            e => e.Should().Be("Id"),
                            e => e.Should().Be("CurrencyCode"),
                            e => e.Should().Be("CurrencyName"),
                            e => e.Should().Be("NumericCode"),
                            e => e.Should().Be("Description")
                        );

                        first.Values.Should().SatisfyRespectively(
                            e => e.Should().Be(1),
                            e => e.Should().Be("EUR"),
                            e => e.Should().Be("Euro"),
                            e => e.Should().Be(978),
                            e => e.Should().Be("Foo")
                        );
                    },
                    second =>
                    {
                        second.Values.Should().SatisfyRespectively(
                            e => e.Should().Be(2),
                            e => e.Should().Be("USD"),
                            e => e.Should().Be("US Dollars"),
                            e => e.Should().Be(840),
                            e => e.Should().Be("Foo")
                        );
                    });
        }

        [Fact]
        public async Task Visit_CsvSeedDeclarationNode_NonNullableAttributeHasNoSeedDataMappingAndDefaultValue_ShouldAddErrorToRepository()
        {
            // Arrange
            var entityModel = ArrangeEntityModel();
            _modelRepository.Add(entityModel);

            var node = new CsvSeedDeclarationNode(new EntityDeclarationNode(new ModelNode(), entityModel.Name),
                new Uri("https://currencycode"), true, ";",
                new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    {"CurrencyCode", "Code"},
                });

            // Act
            await _sut.Visit(node);

            // Assert
            _errorRepository.GetErrors().Should().SatisfyRespectively(
                first => first.Should().Be("Attribute 'CurrencyName' is required but has no seed data mapping"),
                second => second.Should().Be("Attribute 'NumericCode' is required but has no seed data mapping"));
        }

        public EntityModel ArrangeEntityModel()
        {
            var entityModel = new EntityModel("Currency");
            entityModel.CodeAttributes.Add(new AttributeModel("Id", "int", false, null));
            entityModel.CodeAttributes.Add(new AttributeModel("CurrencyCode", "string", false, null));
            entityModel.CodeAttributes.Add(new AttributeModel("CurrencyName", "string", false, null));
            entityModel.CodeAttributes.Add(new AttributeModel("NumericCode", "int", false, null));
            entityModel.CodeAttributes.Add(new AttributeModel("Description", "string", false, "Foo"));
            entityModel.CodeAttributes.Add(new AttributeModel("Comments", "string", true, null));

            return entityModel;
        }
    }
}
