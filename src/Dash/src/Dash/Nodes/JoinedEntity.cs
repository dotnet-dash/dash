using System.Collections.Generic;

namespace Dash.Nodes
{
    public class JoinedEntity : Entity
    {
        public JoinedEntity(Entity entityA, Entity entityB) : base(entityA.Name + entityB.Name)
        {
            SingleReferences.Add(new KeyValuePair<string, Entity>(entityA.Name, entityA));
            SingleReferences.Add(new KeyValuePair<string, Entity>(entityB.Name, entityB));
        }
    }
}