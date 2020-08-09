using System;
using System.Linq;
using System.Text.Json;
using Dash.Engine.Abstractions;
using Dash.Extensions;
using Dash.Nodes;
using Attribute = Dash.Nodes.Attribute;

namespace Dash.Engine.JsonParser
{
    public class JsonParser : IParser
    {
        private Model? _currentModel;

        public Model Parse(string sourceCode)
        {
            _currentModel = new Model();

            var document = JsonDocument.Parse(sourceCode);
            var modelProperty = GetModelProperty(document.RootElement);
            if (modelProperty.HasValue)
            {
                var entities = modelProperty.Value.Value.EnumerateObject();
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

            return _currentModel;
        }

        private JsonProperty? GetModelProperty(JsonElement jsonElement)
        {
            foreach (var @object in jsonElement.EnumerateObject())
            {
                if (@object.Name.IsSame("Model"))
                {
                    return @object;
                }
            }

            return null;
        }

        private void ProcessBase()
        {
            var defaultBase = new Entity("Base")
            {
                Inherits = null
            };
            defaultBase.Attributes.Add(new Attribute("Id", "Int"));

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
                    ProcessHasAndBelongsToMany(property, currentEntity);
                }
            }
        }

        private void ProcessRelationshipProperty(JsonProperty property, Entity currentEntity, string relationshipProperty, Action<Entity> referencedEntityAction)
        {
            if (!property.Name.IsSame(relationshipProperty))
            {
                return;
            }

            if (property.Value.ValueKind != JsonValueKind.Array)
            {
                _currentModel!.Errors.Add($"The value of the '{currentEntity.Name}/{relationshipProperty}' property must be a JSON array");
                return;
            }

            var referencedEntities = property.Value.EnumerateArray();
            foreach (var item in referencedEntities)
            {
                var entityName = item.GetString();

                var referencedEntity = _currentModel!.Entities.FirstOrDefault(e => e.Name.IsSame(entityName));
                if (referencedEntity == null)
                {
                    _currentModel!.Errors.Add($"Could not find the referenced Entity '{entityName}'");
                }
                else
                {
                    referencedEntityAction(referencedEntity);
                }
            }
        }

        private void ProcessHas(JsonProperty property, Entity currentEntity)
        {
            ProcessRelationshipProperty(property, currentEntity, "@@Has Many", referencedEntity =>
            {
                currentEntity.CollectionReferences.Add(referencedEntity);
                referencedEntity.SingleReferences.Add(currentEntity);
            });
        }

        private void ProcessHasAndBelongsToMany(JsonProperty property, Entity currentEntity)
        {
            ProcessRelationshipProperty(property, currentEntity, "@@Has And Belongs To Many", referencedEntity =>
            {
                var joinedEntities = new JoinedEntity(currentEntity, referencedEntity);
                currentEntity.CollectionReferences.Add(joinedEntities);
                referencedEntity.CollectionReferences.Add(joinedEntities);
                _currentModel!.Entities.Add(joinedEntities);
            });
        }

        private void TraverseEntities(JsonProperty jsonProperty)
        {
            if (jsonProperty.Value.ValueKind == JsonValueKind.Object)
            {
                var currentEntity = new Entity(jsonProperty.Name);
                _currentModel!.Entities.Add(currentEntity);

                var elements = jsonProperty.Value.EnumerateObject();

                foreach (var e in elements)
                {
                    TraverseEntities(e);
                }

                TraverseAttributes(elements, currentEntity);
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
                        entity.Attributes.Add(new Attribute(@object.Name, @object.Value.GetString()));
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
