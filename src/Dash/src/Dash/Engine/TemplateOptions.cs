// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using Dash.Engine.Models;

namespace Dash.Engine
{
    public class TemplateOptions
    {
        public TemplateOptions(string templateText, IEnumerable<EntityModel> entities, bool pluralize)
        {
            TemplateText = templateText;
            Entities = entities;
            Pluralize = pluralize;
        }

        public string TemplateText { get; }
        public IEnumerable<EntityModel> Entities { get; }
        public bool Pluralize { get; }
    }
}