namespace Dash.Engine.Models
{
    public class JoinedEntityModel : EntityModel
    {
        public JoinedEntityModel(string entityNameA, string entityNameB) : base(entityNameA + entityNameB)
        {
        }
    }
}
