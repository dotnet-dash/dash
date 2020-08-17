using System.Collections.Generic;
using System.Linq;
using Dash.Extensions;

namespace Dash.Engine.Models
{
    public class EntityModel
    {
        public EntityModel(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public bool CodeGeneration => !Name.IsSame("Base");

        public IList<AttributeModel> CodeAttributes { get; } = new List<AttributeModel>();
        public IList<AttributeModel> DataAttributes { get; } = new List<AttributeModel>();

        public IList<ReferencedEntityModel> SingleReferences { get; } = new List<ReferencedEntityModel>();

        public IList<ReferencedEntityModel> CollectionReferences { get; } = new List<ReferencedEntityModel>();

        public void InheritAttributes(EntityModel superEntity)
        {
            var attributeNames = CodeAttributes
                .Select(e => e.Name)
                .ToList();

            foreach (var superAttribute in superEntity.CodeAttributes.Reverse())
            {
                if (!attributeNames.Has(superAttribute.Name))
                {
                    CodeAttributes.Insert(0, superAttribute);
                }
                else
                {
                    var overriddenAttributes = CodeAttributes
                        .Where(e => e.Name.IsSame(superAttribute.Name))
                        .ToList();

                    foreach (var item in overriddenAttributes)
                    {
                        CodeAttributes.Remove(item);
                        CodeAttributes.Insert(0, item);
                    }
                }
            }
        }
    }
}
