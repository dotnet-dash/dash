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
        [Fact]
        public async Task Visit_CsvSeedDeclarationNode_ShouldHaveAddedSeedDataToEntity()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();

            var csvContent = new StringBuilder();
            csvContent.AppendLine("Code;Name;NumericCode");
            csvContent.AppendLine("EUR;Euro;978");
            csvContent.AppendLine("USD;US Dollars;840");
            mockFileSystem.AddFile("c:\\currencies.csv", new MockFileData(csvContent.ToString()));

            var entityModel = new EntityModel("Currency");
            entityModel.CodeAttributes.Add(new AttributeModel("CurrencyCode", "string", false, null));
            entityModel.CodeAttributes.Add(new AttributeModel("CurrencyName", "string", false, null));
            entityModel.CodeAttributes.Add(new AttributeModel("NumericCode", "int", false, null));

            var modelRepository = new DefaultModelRepository();
            modelRepository.Add(entityModel);

            var errorRepository = new ErrorRepository();

            var uriResourceRepository = Substitute.For<IUriResourceRepository>();
            uriResourceRepository.Get(new Uri("https://currencycode")).Returns("c:\\currencies.csv");

            var sut = new ModelSeedBuilder(Substitute.For<IConsole>(),
                mockFileSystem,
                modelRepository,
                errorRepository,
                uriResourceRepository);

            var parent = new EntityDeclarationNode(new ModelNode(), "Currency");

            var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                {"Code", "CurrencyCode"},
                {"Name", "CurrencyName"},
                {"NumericCode", "NumericCode"}
            };
            var node = new CsvSeedDeclarationNode(parent, new Uri("https://currencycode"), true, ";", dictionary);

            // Act
            await sut.Visit(node);

            // Assert
            errorRepository.HasErrors().Should().BeFalse();

            modelRepository
                .Get("Currency")
                .SeedData.Should().SatisfyRespectively(
                    first =>
                    {
                        first.Should().SatisfyRespectively(
                            a => a.Key.Should().Be("CurrencyCode"),
                            b => b.Key.Should().Be("CurrencyName"),
                            c => c.Key.Should().Be("NumericCode")
                        );

                        first.Should().SatisfyRespectively(
                            a => a.Value.Should().Be("EUR"),
                            b => b.Value.Should().Be("Euro"),
                            c => c.Value.Should().Be(978)
                        );
                    },
                    second =>
                    {
                        second.Should().SatisfyRespectively(
                            a => a.Value.Should().Be("USD"),
                            b => b.Value.Should().Be("US Dollars"),
                            c => c.Value.Should().Be(840)
                        );
                    });
        }
    }
}
