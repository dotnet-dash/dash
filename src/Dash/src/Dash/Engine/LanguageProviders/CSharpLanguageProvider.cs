namespace Dash.Engine.LanguageProviders
{
    public class CSharpLanguageProvider : BaseLanguageProvider
    {
        public override string Name => "cs";
        public override string Int => "int";
        public override string Bool => "bool";
        public override string String => "string";
        public override string Unicode => "string";
    }
}
