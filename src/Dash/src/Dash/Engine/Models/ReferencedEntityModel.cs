namespace Dash.Engine.Models
{
    public class ReferencedEntityModel
    {
        public ReferencedEntityModel(string referenceName, string entityModel, bool isNullable)
        {
            ReferenceName = referenceName;
            EntityModel = entityModel;
            IsNullable = isNullable;
        }

        public string ReferenceName { get; }

        public string EntityModel { get; }

        public bool IsNullable { get; }
    }
}