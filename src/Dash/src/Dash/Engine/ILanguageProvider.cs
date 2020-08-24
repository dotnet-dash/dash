// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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
