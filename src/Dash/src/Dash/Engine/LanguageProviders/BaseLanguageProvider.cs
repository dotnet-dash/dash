using Dash.Engine.Abstractions;
using Dash.Exceptions;

namespace Dash.Engine.LanguageProviders
{
    public abstract class BaseLanguageProvider : ILanguageProvider
    {
        public abstract string Name { get; }
        public abstract string Int { get; }
        public abstract string Bool { get; }
        public abstract string Email { get; }
        public abstract string Guid { get; }
        public abstract string String { get; }
        public abstract string Unicode { get; }

        public string Translate(string dashDataType)
        {
            switch (dashDataType.ToLower())
            {
                case "int":
                    return Int;

                case "bool":
                    return Bool;

                case "email":
                    return Email;

                case "guid":
                    return Guid;

                case "string":
                    return String;

                case "unicode":
                    return Unicode;

                default:
                    throw new InvalidDataTypeException(dashDataType);
            }
        }
    }
}