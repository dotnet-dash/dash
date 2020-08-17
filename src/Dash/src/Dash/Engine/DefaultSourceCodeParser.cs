using System;
using System.Text.Json;
using Dash.Engine.Abstractions;
using Dash.Engine.Models.SourceCode;
using Dash.Extensions;
using Dash.Nodes;

namespace Dash.Engine
{
    public class DefaultSourceCodeParser : ISourceCodeParser
    {
        public SourceCodeDocument Parse(string sourceCode)
        {
            var document = JsonDocument.Parse(sourceCode);

            var configuration = ParseConfiguration(document);
            var modelNode = ParseModel(document);

            return new SourceCodeDocument(configuration, modelNode);
        }

        private Configuration ParseConfiguration(JsonDocument document)
        {
            if (!document.RootElement.TryGetProperty("Configuration", out var configurationProperty))
            {
                return new Configuration();
            }

            var configurationSourceCode = configurationProperty.GetRawText();
            return JsonSerializer.Deserialize<Configuration>(configurationSourceCode);
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

        private void TraverseModelEntities(ModelNode modelNode, JsonProperty entityObject)
        {
            if (entityObject.Value.ValueKind == JsonValueKind.Object)
            {
                if (!entityObject.Name.StartsWith("@@"))
                {
                    var entityDeclarationNode = modelNode.AddEntityDeclarationNode(entityObject.Name);

                    var entityObjectProperties = entityObject.Value.EnumerateObject();
                    TraverseAttributes(entityDeclarationNode, entityObjectProperties);
                    TraverseRelationshipProperties(entityDeclarationNode, entityObjectProperties);
                }
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
                }
            }
        }
    }
}
