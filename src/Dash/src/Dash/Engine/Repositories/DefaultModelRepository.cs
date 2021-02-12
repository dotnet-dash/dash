// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Dash.Engine.Models;
using Dash.Exceptions;
using Dash.Extensions;

namespace Dash.Engine.Repositories
{
    public class DefaultModelRepository : IModelRepository
    {
        private readonly IList<EntityModel> _entityModels = new List<EntityModel>();

        public IEnumerable<EntityModel> EntityModels => _entityModels;

        public void CreateEntityModel(params string[] entityModelNames)
        {
            foreach (var name in entityModelNames)
            {
                Add(new EntityModel(name));
            }
        }

        public void Add(EntityModel entityModel)
        {
            if (TryGet(entityModel.Name, out _))
            {
                throw new InvalidOperationException("Cannot add a duplicate EntityModel");
            }

            _entityModels.Add(entityModel);
        }

        public virtual EntityModel Get(string entityName)
        {
            if (!TryGet(entityName, out var entity))
            {
                throw new EntityModelNotFoundException($"Entity '{entityName}' was not found in the repository");
            }

            return entity!;
        }

        public bool TryGet(string entityName, out EntityModel? entityModel)
        {
            entityModel = _entityModels.SingleOrDefault(e => e.Name.IsSame(entityName));
            return entityModel != null;
        }
    }
}
