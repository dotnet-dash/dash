﻿// Copyright (c) Huy Hoang. All rights reserved.
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
            csvContent.AppendLine("Code;Name");
            csvContent.AppendLine("EUR;Euro");
            csvContent.AppendLine("USD;US Dollars");
            mockFileSystem.AddFile("c:\\currencies.csv", new MockFileData(csvContent.ToString()));

            var modelRepository = new DefaultModelRepository();
            modelRepository.Add(new EntityModel("CurrencyCode"));

            var errorRepository = new ErrorRepository();

            var uriResourceRepository = NSubstitute.Substitute.For<IUriResourceRepository>();
            uriResourceRepository.Get(new Uri("https://currencycode")).Returns("c:\\currencies.csv");

            var sut = new ModelSeedBuilder(NSubstitute.Substitute.For<IConsole>(),
                mockFileSystem,
                modelRepository,
                errorRepository,
                uriResourceRepository);

            var parent = new EntityDeclarationNode(new ModelNode(), "CurrencyCode");
            var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                {"Code", "CurrencyCode"},
                {"Name", "CurrencyName"}
            };
            var node = new CsvSeedDeclarationNode(parent, new Uri("https://currencycode"), true, ";", dictionary);

            // Act
            await sut.Visit(node);

            // Assert
            errorRepository.HasErrors().Should().BeFalse();

            modelRepository
                .Get("CurrencyCode")
                .SeedData.Should().SatisfyRespectively(
                    first =>
                    {
                        first.Keys.Should().SatisfyRespectively(
                            a => a.Should().Be("CurrencyCode"),
                            b => b.Should().Be("CurrencyName"));

                        first.Values.Should().SatisfyRespectively(
                            a => a.Should().Be("EUR"),
                            b => b.Should().Be("Euro"));
                    },
                    second =>
                    {
                        second.Values.Should().SatisfyRespectively(
                            a => a.Should().Be("USD"),
                            b => b.Should().Be("US Dollars"));
                    });
        }
    }
}
