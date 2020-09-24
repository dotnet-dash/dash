// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Dash.Common;
using Dash.Exceptions;
using Dash.Extensions;
using Dash.Nodes;

#pragma warning disable S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
namespace Dash.Engine.Parsers
{
    public class DefaultSourceCodeParser : ISourceCodeParser
    {
        private readonly IFileService _fileService;

        public DefaultSourceCodeParser(IFileService fileService)
        {
            _fileService = fileService;
        }

        public SourceCodeNode Parse(string sourceCode)
        {
            try
            {
                var document = JsonDocument.Parse(sourceCode);

                var configuration = ParseConfiguration(document);
                var modelNode = ParseModel(document);

                return new SourceCodeNode(configuration, modelNode);
            }
            catch (JsonException exception)
            {
                throw new ParserException($"JSON error: {exception.Message}");
            }
        }

        private ConfigurationNode ParseConfiguration(JsonDocument document)
        {
            if (!document.RootElement.TryGetProperty("Configuration", out var configurationProperty))
            {
                return new ConfigurationNode()
                    .AddTemplateNode("dash://efpoco", _fileService.AbsoluteWorkingDirectory)
                    .AddTemplateNode("dash://efcontext", _fileService.AbsoluteWorkingDirectory);
            }

            var configurationSourceCode = configurationProperty.GetRawText();

            var deserialized = JsonSerializer.Deserialize<ConfigurationNode>(configurationSourceCode);

            foreach (var item in deserialized.Templates)
            {
                if (item.Output == null)
                {
                    item.Output = _fileService.AbsoluteWorkingDirectory;
                }
            }

            return deserialized;
        }

        private ModelNode ParseModel(JsonDocument document)
        {
            var result = new ModelNode();

            if (!document.RootElement.TryGetProperty("Model", out var modelProperty))
            {
                return result;
            }

            var entityObjects = modelProperty.EnumerateObject();
            foreach (var entityObject in entityObjects)
            {
                TraverseModelEntities(result, entityObject);
            }

            return result;
        }

        private void TraverseRelationshipProperties(EntityDeclarationNode entityDeclarationNode, JsonElement.ObjectEnumerator objectProperties)
        {
            objectProperties.Reset();

            foreach (var property in objectProperties)
            {
                ProcessRelationshipProperty(property, "@@Has", (name, referencedEntity) =>
                    {
                        entityDeclarationNode.Has.Add(new HasReferenceDeclarationNode(entityDeclarationNode, name, referencedEntity));
                    });

                ProcessRelationshipProperty(property, "@@Has Many", (name, referencedEntity) =>
                    {
                        entityDeclarationNode.HasMany.Add(new HasManyReferenceDeclarationNode(entityDeclarationNode, name, referencedEntity));
                    });

                ProcessRelationshipProperty(property, "@@Has And Belongs To Many", (name, referencedEntity) =>
                    {
                        entityDeclarationNode.HasAndBelongsToMany.Add(new HasAndBelongsToManyDeclarationNode(entityDeclarationNode, name, referencedEntity));
                    });
            }
        }

        private void ProcessRelationshipProperty(JsonProperty objectProperty, string relationship, Action<string, string> func)
        {
            if (objectProperty.Name.IsSame(relationship))
            {
                if (objectProperty.Value.ValueKind == JsonValueKind.Object)
                {
                    foreach (var hasProperty in objectProperty.Value.EnumerateObject())
                    {
                        func(hasProperty.Name, hasProperty.Value.GetString());
                    }
                }
                else if (objectProperty.Value.ValueKind == JsonValueKind.Array)
                {
                    foreach (var hasProperty in objectProperty.Value.EnumerateArray())
                    {
                        func(hasProperty.GetString(), hasProperty.GetString());
                    }
                }
            }
        }

        private void TryTraverseSeed(EntityDeclarationNode entityDeclarationNode, JsonElement.ObjectEnumerator entityObjectProperties)
        {
            foreach (var item in entityObjectProperties.Where(e => e.Name.IsSame("@@Seed")))
            {
                if (item.Value.TryGetProperty("FromCsv", out var csvElement))
                {
                    if (csvElement.ValueKind != JsonValueKind.Object)
                    {
                        throw new ParserException("The 'FromCsv' value must be an Object");
                    }

                    var uri = new Uri(csvElement.GetProperty("Uri").GetString());

                    var firstLineIsHeader = false;
                    if (csvElement.TryGetProperty("FirstLineIsHeader", out var value))
                    {
                        firstLineIsHeader = value.GetBoolean();
                    }

                    var rawText = csvElement.GetProperty("MapHeaders").GetRawText();
                    var mapHeaders = JsonSerializer.Deserialize<IDictionary<string, string>>(rawText);

                    string? delimiter = null;
                    if (csvElement.TryGetProperty("Delimiter", out var delimiterJsonElement))
                    {
                        delimiter = delimiterJsonElement.GetString();
                    }

                    entityDeclarationNode.AddCsvSeedDeclarationNode(uri, firstLineIsHeader, delimiter, mapHeaders);
                    continue;
                }

                throw new ParserException("Missing 'FromCsv' or 'FromData' property inside @@Seed");
            }
        }

        private void TraverseModelEntities(ModelNode modelNode, JsonProperty entityObject)
        {
            if (entityObject.Value.ValueKind == JsonValueKind.Object && !entityObject.Name.StartsWith("@@"))
            {
                var entityDeclarationNode = modelNode.AddEntityDeclarationNode(entityObject.Name);

                var entityObjectProperties = entityObject.Value.EnumerateObject();
                TraverseAttributes(entityDeclarationNode, entityObjectProperties);
                TraverseRelationshipProperties(entityDeclarationNode, entityObjectProperties);
                TryTraverseSeed(entityDeclarationNode, entityObjectProperties);
            }
        }

        private void TraverseAttributes(EntityDeclarationNode entityDeclarationNode, JsonElement.ObjectEnumerator attributes)
        {
            attributes.Reset();
            foreach (var attribute in attributes)
            {
                if (!attribute.Name.StartsWith("@@"))
                {
                    if (attribute.Value.ValueKind == JsonValueKind.String)
                    {
                        entityDeclarationNode.AddAttributeDeclaration(attribute.Name, attribute.Value.GetString());
                    }
                }
                else
                {
                    if (attribute.Name.IsSame("@@Inherits"))
                    {
                        entityDeclarationNode.AddInheritanceDeclaration(attribute.Value.GetString());
                    }
                    else if (attribute.Name.IsSame("@@Abstract"))
                    {
                        entityDeclarationNode.AddAbstractDeclarationNode(attribute.Value.GetBoolean());
                    }
                }
            }
        }
    }
}
#pragma warning restore S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
