// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using CsvHelper.TypeConversion;
using Dash.Engine.LanguageProviders;

namespace Dash.Engine.Utilities
{
    public class TypeConverterDictionary : ReadOnlyDictionary<string, ITypeConverter>
    {
        private static readonly CSharpLanguageProvider CSharpLanguageProvider = new CSharpLanguageProvider();

        public TypeConverterDictionary() : base(new Dictionary<string, ITypeConverter>
        {
            [CSharpLanguageProvider.Int] = new Int32Converter(),
            [CSharpLanguageProvider.String] = new StringConverter(),
        })
        {
        }
    }
}