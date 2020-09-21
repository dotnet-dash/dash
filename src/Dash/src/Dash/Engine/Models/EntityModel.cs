// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Dash.Constants;
using Dash.Extensions;

namespace Dash.Engine.Models
{
    public class EntityModel
    {
        public EntityModel(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public bool IsAbstract { get; set; }

        public IList<AttributeModel> CodeAttributes { get; } = new List<AttributeModel>();
        public IList<AttributeModel> DataAttributes { get; } = new List<AttributeModel>();

        public IList<ReferencedEntityModel> SingleReferences { get; } = new List<ReferencedEntityModel>();

        public IList<ReferencedEntityModel> CollectionReferences { get; } = new List<ReferencedEntityModel>();

        public IList<IDictionary<string, object>> SeedData { get; } = new List<IDictionary<string, object>>();

        public void InheritAttributes(EntityModel superEntity)
        {
            var attributeNames = CodeAttributes
                .Select(e => e.Name)
                .ToList();

            foreach (var superAttribute in superEntity.CodeAttributes.Reverse())
            {
                if (!attributeNames.Has(superAttribute.Name))
                {
                    CodeAttributes.Insert(0, superAttribute);
                }
                else
                {
                    var overriddenAttributes = CodeAttributes
                        .Where(e => e.Name.IsSame(superAttribute.Name))
                        .ToList();

                    foreach (var item in overriddenAttributes)
                    {
                        CodeAttributes.Remove(item);
                        CodeAttributes.Insert(0, item);
                    }
                }
            }
        }

        public IEnumerable<AttributeModel> WithoutIdAttribute() =>
            CodeAttributes.Where(e => !e.Name.IsSame(DashModelFileConstants.BaseEntityIdAttributeName));
    }
}
