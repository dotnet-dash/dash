// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Dash.Constants;
using Dash.Exceptions;

namespace Dash.Engine.LanguageProviders
{
    public abstract class BaseLanguageProvider : ILanguageProvider
    {
        public abstract string Name { get; }
        public abstract string Int { get; }
        public abstract string Bool { get; }
        public abstract string DateTime { get; }
        public abstract string Email { get; }
        public abstract string Guid { get; }
        public abstract string String { get; }
        public abstract string Unicode { get; }

        public string Translate(string dashDataType)
        {
            switch (dashDataType.ToLower())
            {
                case DashModelFileConstants.DataTypeInteger:
                    return Int;

                case "bool":
                    return Bool;

                case "datetime":
                    return DateTime;

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