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
            var localCopy = await _uriResourceRepository.Get(node.UriNode.Uri);
            using var reader = new StreamReader(_fileSystem.FileStream.Create(localCopy, FileMode.Open));
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Configuration.MissingFieldFound = (headerNames, index, context) =>
            {
                _errorRepository.Add(context.Field);
            };
            csv.Configuration.HasHeaderRecord = node.FirstLineIsHeader;
            csv.Configuration.Delimiter = node.Delimiter;

            int line = 0;

            if (node.FirstLineIsHeader)
            {
                line++;
                await csv.ReadAsync();
                csv.ReadHeader();
            }

            var entityModel = _modelRepository.Get(node.Parent.Name);

            while (await csv.ReadAsync())
            {
                line++;

                var seedDataRow = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                foreach (var (csvHeader, entityAttributeName) in node.MapHeaders)
                {
                    var nativeDataType = entityModel.CodeAttributes
                        .First(e => e.Name.IsSame(entityAttributeName))
                        .DataType;

                    if (csv.TryGetField<object>(csvHeader, _typeConverters[nativeDataType], out var value))
                    {
                        seedDataRow.Add(entityAttributeName, value);
                    }
                    else
                    {
                        // Let user configure what to do if value cannot be converted
                    }
                }

                entityModel.SeedData.Add(seedDataRow);
            }

            await base.Visit(node);
        }
    }
}
