// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace Dash.Engine.Repositories
{
    public class ErrorRepository : IErrorRepository
    {
        private readonly List<string> _errors = new List<string>();

        public bool HasErrors()
        {
            return _errors.Any();
        }

        public IEnumerable<string> GetErrors()
        {
            return _errors;
        }

        public void Add(string error)
        {
            _errors.Add(error);
        }
    }
}