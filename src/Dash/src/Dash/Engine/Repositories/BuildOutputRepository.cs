// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Dash.Engine.Repositories
{
    public class BuildOutputRepository : IBuildOutputRepository
    {
        private readonly HashSet<BuildOutput> _outputs = new HashSet<BuildOutput>();

        public void Add(string path, string content)
        {
            if (!_outputs.Add(new BuildOutput(path, content)))
            {
                throw new InvalidOperationException($"Build output '{path}' already added to repository");
            }
        }

        public void Update(BuildOutput item)
        {
            if (!_outputs.Remove(item))
            {
                throw new InvalidOperationException($"Build output '{item.Path} does not exist in repository");
            }

            Add(item.Path, item.GeneratedSourceCodeContent);
        }

        public IEnumerable<BuildOutput> GetOutputItems()
        {
            return _outputs.ToList();
        }
    }
}
