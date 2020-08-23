namespace Dash.Engine
{
    public interface ILanguageProvider
    {
        string Name { get; }
        string Int { get; }
        string Bool { get; }
        string DateTime { get; }
        string Guid { get; }
        string String { get; }
        string Unicode { get; }
        public string Translate(string dashDataType);
    }
}
