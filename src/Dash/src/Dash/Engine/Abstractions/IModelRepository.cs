using System.Collections.Generic;
using Dash.Engine.Models;

namespace Dash.Engine.Abstractions
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