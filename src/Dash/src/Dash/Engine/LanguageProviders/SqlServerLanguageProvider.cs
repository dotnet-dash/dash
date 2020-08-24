// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Dash.Engine.LanguageProviders
{
    public class SqlServerLanguageProvider : BaseLanguageProvider
    {
        public override string Name => "sqlserver";
        public override string Int => "int";
        public override string Bool => "bit";
        public override string DateTime => "datetime";
        public override string Email => "nvarchar";
        public override string Guid => "uniqueidentifier";
        public override string String => "varchar";
        public override string Unicode => "nvarchar";
    }
}
