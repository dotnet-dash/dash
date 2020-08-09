namespace Dash.Nodes
{
    public class JoinedEntity : Entity
    {
        public JoinedEntity(Entity entityA, Entity entityB) : base(entityA.Name + entityB.Name)
        {
            SingleReferences = new[] {entityA, entityB};
        }
    }
}