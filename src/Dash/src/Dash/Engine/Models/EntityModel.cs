using System.Collections.Generic;

namespace Dash.Engine.Models
{
    public class EntityModel
    {
        public EntityModel(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public IList<AttributeModel> CodeAttributes { get; } = new List<AttributeModel>();
        public IList<AttributeModel> DataAttributes { get; } = new List<AttributeModel>();

        public IList<ReferencedEntityModel> SingleReferences { get; } = new List<ReferencedEntityModel>();

        public IList<ReferencedEntityModel> CollectionReferences { get; } = new List<ReferencedEntityModel>();
    }
}
