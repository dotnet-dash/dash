using System;
using System.Text.Json;
using Dash.Engine.Abstractions;
using Dash.Extensions;
using Dash.Nodes;

namespace Dash.Engine.JsonParser
{
    public class JsonParser : IParser2
    {
        private readonly DataTypeParser _dataTypeParser;

        public JsonParser(DataTypeParser dataTypeParser)
        {
            _dataTypeParser = dataTypeParser;
        }

        public ModelNode Parse(string sourceCode)
        {
            var currentModel = new ModelNode();

            var document = JsonDocument.Parse(sourceCode);

            // ParseConfiguration(document);
            ParseModel(currentModel, document);

            return currentModel;
        }

        private void ParseModel(ModelNode modelNode, JsonDocument document)
        {
            if (!document.RootElement.TryGetProperty("Model", out var modelProperty))
            {
                return;
            }

            var entityObjects = modelProperty.EnumerateObject();
            foreach (var entityObject in entityObjects)
            {
                TraverseModelEntities(modelNode, entityObject);
            }
        }

        private void TraverseRelationshipProperties(EntityDeclarationNode entityDeclarationNode, JsonElement.ObjectEnumerator objectProperties)
        {
            objectProperties.Reset();

            foreach (var property in objectProperties)
            {
                ProcessRelationshipProperty(property, "@@Has", node => entityDeclarationNode.SingleEntityReferences.Add(node));
                ProcessRelationshipProperty(property, "@@Has Many", node => entityDeclarationNode.CollectionEntityReferences.Add(node));
                ProcessRelationshipProperty(property, "@@Has And Belongs To Many", node => entityDeclarationNode.CollectionEntityReferences.Add(node));
            }
        }

        private void ProcessRelationshipProperty(JsonProperty objectProperty, string relationship, Action<ReferenceDeclarationNode> func)
        {
            if (objectProperty.Name.IsSame(relationship))
            {
                if (objectProperty.Value.ValueKind == JsonValueKind.Object)
                {
                    foreach (var hasProperty in objectProperty.Value.EnumerateObject())
                    {
                        func(new ReferenceDeclarationNode(hasProperty.Name, hasProperty.Value.GetString()));
                    }
                }
                else if (objectProperty.Value.ValueKind == JsonValueKind.Array)
                {
                    foreach (var hasProperty in objectProperty.Value.EnumerateArray())
                    {
                        func(new ReferenceDeclarationNode(hasProperty.GetString(), hasProperty.GetString()));
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
                    var entityDeclarationNode = new EntityDeclarationNode(entityObject.Name);
                    modelNode.EntityDeclarations.Add(entityDeclarationNode);

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
                if (attribute.Value.ValueKind == JsonValueKind.String)
                {
                    entityDeclarationNode.AddAttributeDeclaration(attribute.Name, attribute.Value.GetString());
                }
            }
        }
    }
}
