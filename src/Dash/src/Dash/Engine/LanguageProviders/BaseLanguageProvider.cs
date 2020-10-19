// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Dash.Engine.DataTypes;
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

        public string Translate(IDataType dataType)
        {
            return dataType switch
            {
                IntDataType _ => Int,
                BoolDataType _ => Bool,
                DateTimeDataType _ => DateTime,
                EmailDataType _ => Email,
                GuidDataType _ => Guid,
                StringDataType _ => String,
                UnicodeDataType _ => Unicode,
                _ => throw new InvalidDataTypeException(dataType.Name),
            };
        }
    }
}