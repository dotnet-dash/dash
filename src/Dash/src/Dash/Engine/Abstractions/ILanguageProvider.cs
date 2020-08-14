namespace Dash.Engine.Abstractions
{
    public interface ILanguageProvider
    {
        string Name { get; }
        string Int { get; }
        string Bool { get; }
        string Guid { get; }
        string String { get; }
        string Unicode { get; }
        public string Translate(string dashDataType);
    }
}
