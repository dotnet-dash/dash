using System;
using System.Collections.Generic;
using System.Linq;
using Dash.Engine.Abstractions;
using Dash.Engine.Models;
using Dash.Extensions;

namespace Dash.Engine
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
                throw new Exception("Cannot add a duplicate EntityModel"); // TODO: introduce new exception type
            }

            _entityModels.Add(entityModel);
        }

        public EntityModel Get(string entityName)
        {
            if (!TryGet(entityName, out var entity))
            {
                throw new Exception($"Entity '{entityName}' was not found in the repository"); // TODO: introduce new exception type
            }

            return entity;
        }

        public bool TryGet(string entityName, out EntityModel entityModel)
        {
            entityModel = _entityModels.SingleOrDefault(e => e.Name.IsSame(entityName));
            return entityModel != null;
        }
    }
}
