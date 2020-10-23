namespace Dash.Engine.DataTypes
{
    public class UnknownDataType : IDataType
    {
        public UnknownDataType(string unknownDataType)
        {
            Name = unknownDataType;
        }

        public string Name { get; }
        public bool IsNumeric => false;
        public bool IsDateTime => false;
        public bool IsBoolean => false;
        public bool IsUnicode => false;
        public bool IsString => false;
    }
}
