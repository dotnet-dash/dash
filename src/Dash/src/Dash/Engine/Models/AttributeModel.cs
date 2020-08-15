namespace Dash.Engine.Models
{
    public class AttributeModel
    {
        public AttributeModel(string name, string dataType, bool isNullable)
        {
            Name = name;
            DataType = dataType;
            IsNullable = isNullable;
        }

        public string Name { get; }

        public string DataType { get; }

        public bool IsNullable { get; }
    }
}