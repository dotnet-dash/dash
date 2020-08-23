using System;
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
        private readonly IUriResourceRepository _uriResourceRepository;

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
