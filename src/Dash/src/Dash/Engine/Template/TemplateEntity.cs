using System.Collections.Generic;
using System.Linq;
using Dash.Nodes;

namespace Dash.Engine.Template
{
    public class TemplateEntity
    {
        private readonly Entity _entity;

        public string Name => _entity.Name;
        public IEnumerable<TemplateEntity> SingleReferences => _entity.SingleReferences.Select(e => new TemplateEntity(e));
        public IEnumerable<TemplateEntity> CollectionReferences => _entity.CollectionReferences.Select(e => new TemplateEntity(e));

        public TemplateEntity(Entity entity)
        {
            _entity = entity;
        }
    }
}
