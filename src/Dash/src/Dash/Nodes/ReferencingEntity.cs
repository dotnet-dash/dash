namespace Dash.Nodes
{
    public class ReferencingEntity
    {
        public ReferencingEntity(Entity entity) : this(entity.Name, entity)
        {
        }

        public ReferencingEntity(string name, Entity entity) : this(name, entity, false)
        {
        }

        public ReferencingEntity(string name, Entity entity, bool isNullable)
        {
            Name = name;
            Entity = entity;
            IsNullable = isNullable;
        }

        public string Name { get; }

        public bool IsNullable { get; }

        public Entity Entity { get; }
    }
}
