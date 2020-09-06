// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Dash.Extensions;

namespace Dash.Engine
{
    public sealed class BuildOutput
    {
        public BuildOutput(string path, string generatedSourceCodeContent)
        {
            Path = path.AbsolutePath();
            GeneratedSourceCodeContent = generatedSourceCodeContent;
        }

        public string Path { get; }

        public string GeneratedSourceCodeContent { get; }

        public bool Equals(BuildOutput other)
        {
            return Path == other.Path;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((BuildOutput) obj);
        }

        public override int GetHashCode()
        {
            return Path.GetHashCode();
        }
    }
}