// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using Dash.Engine.Models;

namespace Dash.Engine
{
    public interface IModelRepository
    {
        IEnumerable<EntityModel> EntityModels { get; }

        void CreateEntityModel(params string[] entityModelNames);

        void Add(EntityModel entityModel);

        EntityModel Get(string entityName);

        bool TryGet(string entityName, out EntityModel entityModel);
    }
}