using System.Collections.Generic;
using System.Linq;
using Dash.Extensions;

namespace Dash.Nodes
{
    public class Entity
    {
        public Entity(string name)
        {
            Name = name;
        }

        public string? Inherits { get; set; } = "Base";

        public string Name { get; set; }

        public IList<Attribute> Attributes { get; } = new List<Attribute>();

        /// <summary>
        /// Only relevant for generating POCO
        /// </summary>
        public IList<Entity> SingleReferences { get; set; } = new List<Entity>();

        /// <summary>
        /// Only relevant for generating POCO
        /// </summary>
        public IList<Entity> CollectionReferences { get; set; } = new List<Entity>();

        public void InheritAttributes(Entity superEntity)
        {
            var attributeNames = Attributes
                .Select(e => e.Name!)
                .ToList();

            foreach (var superAttribute in superEntity.Attributes.Reverse())
            {
                if (!attributeNames.Has(superAttribute.Name!))
                {
                    Attributes.Insert(0, superAttribute);
                }
                else
                {
                    var overriddenAttributes = Attributes
                        .Where(e => e.Name!.IsSame(superAttribute.Name))
                        .ToList();

                    foreach (var item in overriddenAttributes)
                    {
                        Attributes.Remove(item);
                        Attributes.Insert(0, item);
                    }
                }
            }
        }
    }
}
