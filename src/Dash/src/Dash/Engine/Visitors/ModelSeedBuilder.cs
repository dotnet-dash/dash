﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;
using CsvHelper;
using Dash.Engine.Abstractions;
using Dash.Nodes;

namespace Dash.Engine.Visitors
{
    public class ModelSeedBuilder : BaseVisitor
    {
        private readonly IFileSystem _fileSystem;
        private readonly IModelRepository _modelRepository;
        private readonly IErrorRepository _errorRepository;

        public ModelSeedBuilder(IConsole console,
            IFileSystem fileSystem,
            IModelRepository modelRepository,
            IErrorRepository errorRepository) : base(console)
        {
            _fileSystem = fileSystem;
            _modelRepository = modelRepository;
            _errorRepository = errorRepository;
        }

        public override async Task Visit(CsvSeedDeclarationNode node)
        {
            using var reader = new StreamReader(_fileSystem.FileStream.Create(node.UriNode.LocalCopy!, FileMode.Open));
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Configuration.MissingFieldFound = (strings, i, arg3) =>
            {
                _errorRepository.Add(arg3.Field);
            };
            csv.Configuration.HasHeaderRecord = node.FirstLineIsHeader;
            csv.Configuration.Delimiter = node.Delimiter;
            if (node.FirstLineIsHeader)
            {
                await csv.ReadAsync();
                csv.ReadHeader();
            }

            var entityModel = _modelRepository.Get(node.Parent.Name);

            while (await csv.ReadAsync())
            {
                var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (var (csvHeader, entityAttributeName) in node.MapHeaders)
                {
                    var value = csv.GetField<string>(csvHeader);
                    dictionary.Add(entityAttributeName, value);
                }

                entityModel.SeedData.Add(dictionary);
            }

            await base.Visit(node);
        }
    }
}
