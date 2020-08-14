﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dash.Extensions;

namespace Dash.Nodes
{
    public class Entity
    {
        public Entity(string name)
        {
            Name = name;

            Inherits = Name.IsSame("Base") ? null : "Base";
        }

        public string? Inherits { get; set; }

        public string Name { get; set; }

        public IList<Attribute> Attributes { get; } = new List<Attribute>();

        public IList<KeyValuePair<string, Entity>> SingleReferences { get; } = new List<KeyValuePair<string, Entity>>();

        public IList<KeyValuePair<string, Entity>> CollectionReferences { get; } = new List<KeyValuePair<string, Entity>>();

        public void InheritAttributes(Entity superEntity)
        {
            var attributeNames = Attributes
                .Select(e => e.Name!)
                .ToList();

            foreach (var superAttribute in superEntity.Attributes.Reverse())
            {
                if (!attributeNames.Has(superAttribute.Name!))
                {
                    Attributes.Insert(0, superAttribute);
                }
                else
                {
                    var overriddenAttributes = Attributes
                        .Where(e => e.Name!.IsSame(superAttribute.Name))
                        .ToList();

                    foreach (var item in overriddenAttributes)
                    {
                        Attributes.Remove(item);
                        Attributes.Insert(0, item);
                    }
                }
            }
        }
    }
}
