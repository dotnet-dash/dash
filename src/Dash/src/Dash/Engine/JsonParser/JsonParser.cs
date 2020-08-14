using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Dash.Engine.Abstractions;
using Dash.Exceptions;
using Dash.Extensions;
using Dash.Nodes;

namespace Dash.Engine.JsonParser
{
    public class JsonParser : IParser
    {
        private readonly DataTypeParser _dataTypeParser;
        private Model? _currentModel;

        public JsonParser(DataTypeParser dataTypeParser)
        {
            _dataTypeParser = dataTypeParser;
        }

        public Model Parse(string sourceCode)
        {
            _currentModel = new Model();

            var document = JsonDocument.Parse(sourceCode);

            ParseConfiguration(document);
            ParseModel(document);

            return _currentModel;
        }

        private void ParseConfiguration(JsonDocument document)
        {
            if (document.RootElement.TryGetProperty("Configuration", out var configurationProperty))
            {
                _currentModel!.Configuration = JsonSerializer.Deserialize<Configuration>(configurationProperty.GetRawText());
            }
        }

        private void ParseModel(JsonDocument document)
        {
            if (!document.RootElement.TryGetProperty("Model", out var modelProperty))
            {
                return;
            }

            var entities = modelProperty.EnumerateObject();
            foreach (var property in entities)
            {
                TraverseEntities(property);
            }

            ProcessBase();

            entities.Reset();
            foreach (var property in entities)
            {
                TraverseRelationshipProperties(property);
            }

            ProcessInheritance();
        }

        private void ProcessBase()
        {
            var defaultBase = new Entity("Base")
            {
                Inherits = null
            };
            defaultBase.Attributes.Add(_dataTypeParser.Parse("Id", "Int"));

            var baseEntities = _currentModel!.Entities.Where(e => e.Name.IsSame("Base")).ToList();
            if (baseEntities.Count > 0)
            {
                foreach (var entity in baseEntities)
                {
                    entity.InheritAttributes(defaultBase);
                }
            }
            else
            {
                _currentModel.Entities.Insert(0, defaultBase);
            }
        }

        private void ProcessInheritance()
        {
            foreach (var entity in _currentModel!.Entities)
            {
                if (entity.Inherits != null)
                {
                    var super = _currentModel!.Entities.Single(e => e.Name.IsSame(entity.Inherits));
                    entity.InheritAttributes(super);
                }
            }
        }

        private void TraverseRelationshipProperties(JsonProperty jsonProperty)
        {
            if (jsonProperty.Value.ValueKind == JsonValueKind.Object)
            {
                var currentEntity = _currentModel!.Entities.First(e => e.Name.IsSame(jsonProperty.Name));

                var propertiesOfEntity = jsonProperty.Value.EnumerateObject();
                foreach (var property in propertiesOfEntity)
                {
                    ProcessHas(property, currentEntity);
                    ProcessHasMany(property, currentEntity);
                    ProcessHasAndBelongsToMany(property, currentEntity);
                }
            }
        }

        private void ProcessRelationshipProperty(JsonProperty property, Entity currentEntity, string relationshipProperty, Action<string, Entity> referencedEntityAction)
        {
            if (!property.Name.IsSame(relationshipProperty))
            {
                return;
            }

            if (property.Value.ValueKind == JsonValueKind.Array)
            {
                var referencedEntities = property.Value.EnumerateArray();
                foreach (var item in referencedEntities)
                {
                    var referencingEntityName = item.GetString();

                    var referencedEntity = _currentModel!.Entities.FirstOrDefault(e => e.Name.IsSame(referencingEntityName));
                    if (referencedEntity == null)
                    {
                        _currentModel!.Errors.Add($"Could not find the referenced Entity '{referencingEntityName}'");
                    }
                    else
                    {
                        referencedEntityAction(referencingEntityName, referencedEntity);
                    }
                }

                return;
            }

            if (property.Value.ValueKind == JsonValueKind.Object)
            {
                foreach (var item in property.Value.EnumerateObject())
                {
                    var entityName = item.Value.GetString();
                    var referencedEntity = _currentModel!.Entities.FirstOrDefault(e => e.Name.IsSame(entityName));
                    if (referencedEntity == null)
                    {
                        _currentModel!.Errors.Add($"Could not find the referenced Entity '{entityName}'");
                    }
                    else
                    {
                        referencedEntityAction(item.Name, referencedEntity);
                    }
                }

                return;
            }

            _currentModel!.Errors.Add($"The value of the '{currentEntity.Name}/{relationshipProperty}' property must be a JSON array or JSON object but {property.Value.ValueKind} found");
        }

        private void ProcessHas(JsonProperty property, Entity currentEntity)
        {
            ProcessRelationshipProperty(property, currentEntity, "@@Has", (referenceName, referencedEntity) =>
            {
                currentEntity.SingleReferences.Add(new ReferencingEntity(referenceName, referencedEntity));
            });
        }

        private void ProcessHasMany(JsonProperty property, Entity currentEntity)
        {
            ProcessRelationshipProperty(property, currentEntity, "@@Has Many", (referenceName, referencedEntity) =>
            {
                currentEntity.CollectionReferences.Add(new KeyValuePair<string, Entity>(referenceName, referencedEntity));
                referencedEntity.SingleReferences.Add(new ReferencingEntity(currentEntity));
            });
        }

        private void ProcessHasAndBelongsToMany(JsonProperty property, Entity currentEntity)
        {
            ProcessRelationshipProperty(property, currentEntity, "@@Has And Belongs To Many", (referenceName, referencedEntity) =>
            {
                var joinedEntities = new JoinedEntity(currentEntity, referencedEntity);
                currentEntity.CollectionReferences.Add(new KeyValuePair<string, Entity>(referenceName, joinedEntities));
                referencedEntity.CollectionReferences.Add(new KeyValuePair<string, Entity>(currentEntity.Name, joinedEntities));
                _currentModel!.Entities.Add(joinedEntities);
            });
        }

        private void TraverseEntities(JsonProperty jsonProperty)
        {
            if (jsonProperty.Value.ValueKind == JsonValueKind.Object)
            {
                var elements = jsonProperty.Value.EnumerateObject();

                if (!jsonProperty.Name.StartsWith("@@"))
                {
                    var currentEntity = new Entity(jsonProperty.Name);
                    _currentModel!.Entities.Add(currentEntity);
                    TraverseAttributes(elements, currentEntity);
                }

                foreach (var e in elements)
                {
                    TraverseEntities(e);
                }
            }
        }

        private void TraverseAttributes(JsonElement.ObjectEnumerator objects, Entity entity)
        {
            objects.Reset();
            foreach (var @object in objects)
            {
                if (@object.Value.ValueKind != JsonValueKind.Object)
                {
                    if (!@object.Name.StartsWith("@@", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            var attribute = _dataTypeParser.Parse(@object.Name, @object.Value.GetString());
                            entity.Attributes.Add(attribute);
                        }
                        catch (InvalidDataTypeException exception)
                        {
                            _currentModel!.Errors.Add(exception.Message);
                        }
                    }

                    if (@object.Name.Equals("@@Inherits", StringComparison.OrdinalIgnoreCase))
                    {
                        entity.Inherits = @object.Value.GetString();
                    }
                }
            }
        }
    }
}
