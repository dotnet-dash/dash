// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using Dash.Common;
using Dash.Constants;
using Dash.Engine.Models;
using Dash.Engine.Utilities;
using Dash.Extensions;
using Dash.Nodes;

namespace Dash.Engine.Visitors
{
    public class ModelSeedBuilder : BaseVisitor
    {
        private readonly IFileSystem _fileSystem;
        private readonly IModelRepository _modelRepository;
        private readonly IErrorRepository _errorRepository;
        private readonly IUriResourceRepository _uriResourceRepository;
        private readonly TypeConverterDictionary _typeConverters = new TypeConverterDictionary();

        public ModelSeedBuilder(IConsole console,
            IFileSystem fileSystem,
            IModelRepository modelRepository,
            IErrorRepository errorRepository,
            IUriResourceRepository uriResourceRepository) : base(console)
        {
            _fileSystem = fileSystem;
            _modelRepository = modelRepository;
            _errorRepository = errorRepository;
            _uriResourceRepository = uriResourceRepository;
        }

        public override async Task Visit(CsvSeedDeclarationNode node)
        {
            var entityModel = _modelRepository.Get(node.Parent.Name);

            if (!IsMappingValid(node, entityModel))
            {
                return;
            }

            await ReadCsv(node, entityModel);

            await base.Visit(node);
        }

        private async Task ReadCsv(CsvSeedDeclarationNode node, EntityModel entityModel)
        {
            var localCopy = await _uriResourceRepository.Get(node.UriNode.Uri);
            using var reader = new StreamReader(_fileSystem.FileStream.Create(localCopy, FileMode.Open));
            var csvReader = await CreateCsvReader(node, reader);

            if (node.FirstLineIsHeader)
            {
                await csvReader.ReadAsync();
                csvReader.ReadHeader();
            }

            await ProcessCsv(node, entityModel, csvReader);
        }

        private async Task ProcessCsv(
            CsvSeedDeclarationNode node,
            EntityModel entityModel,
            CsvReader csvReader)
        {
            int line = 0;

            int longId = 0; // This needs to be refactored in the future to support long, guid, etc.

            while (await csvReader.ReadAsync())
            {
                line++;
                longId++;

                var seedDataRow = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                {
                    {DashModelFileConstants.BaseEntityIdAttributeName, longId}
                };

                foreach (var attribute in entityModel.WithoutIdAttribute())
                {
                    if (node.MapHeaders.TryGetValue(attribute.Name, out var csvHeader))
                    {
                        if (csvReader.TryGetField<object>(csvHeader, _typeConverters[attribute.TargetEnvironmentDataType], out var value))
                        {
                            seedDataRow.Add(attribute.Name, value);
                        }
                        else
                        {
                            // Let user configure what to do if value cannot be converted
                        }
                    }
                    else if (attribute.DefaultValue != null)
                    {
                        seedDataRow.Add(attribute.Name, attribute.DefaultValue);
                    }
                    else if (!attribute.IsNullable)
                    {
                        _errorRepository.Add("Invalid operation.");
                    }
                }

                entityModel.SeedData.Add(seedDataRow);
            }
        }

        private Task<CsvReader> CreateCsvReader(CsvSeedDeclarationNode node, StreamReader streamReader)
        {
            var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            csv.Configuration.MissingFieldFound = (headerNames, index, context) => { _errorRepository.Add(context.Field); };
            csv.Configuration.HasHeaderRecord = node.FirstLineIsHeader;
            csv.Configuration.Delimiter = node.Delimiter;
            return Task.FromResult(csv);
        }

        private bool IsMappingValid(CsvSeedDeclarationNode node, EntityModel entityModel)
        {
            var result = true;

            var attributes = entityModel.CodeAttributes
                .Where(e => !e.Name.IsSame(DashModelFileConstants.BaseEntityIdAttributeName))
                .Where(e => !e.IsNullable)
                .Where(e => e.DefaultValue == null);

            foreach (var attribute in attributes)
            {
                if (!node.MapHeaders.Keys.Contains(attribute.Name, StringComparer.OrdinalIgnoreCase))
                {
                    _errorRepository.Add($"Attribute '{attribute.Name}' is required but has no seed data mapping");
                    result = false;
                }
            }

            return result;
        }
    }
}
