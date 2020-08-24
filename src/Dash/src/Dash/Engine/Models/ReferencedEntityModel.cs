// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Dash.Engine.Models
{
    public class ReferencedEntityModel
    {
        public ReferencedEntityModel(string referenceName, string entityModel, bool isNullable)
        {
            ReferenceName = referenceName;
            EntityModel = entityModel;
            IsNullable = isNullable;
        }

        public string ReferenceName { get; }

        public string EntityModel { get; }

        public bool IsNullable { get; }
    }
}