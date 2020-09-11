// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Microsoft.CodeAnalysis.CSharp;

namespace Dash.Tests.TestHelpers
{
    public static class StringExtensions
    {
        public static SourceCodeStringAssertions Should(this string instance)
        {
            return new SourceCodeStringAssertions(instance);
        }
    }

    public class SourceCodeStringAssertions : ReferenceTypeAssertions<string, SourceCodeStringAssertions>
    {
        private readonly string _instance;

        protected override string Identifier => "Sourcecode";

        public SourceCodeStringAssertions(string instance)
        {
            _instance = instance;
        }

        public AndConstraint<SourceCodeStringAssertions> HaveSameTree(string sourceCode, string because = "", params object[] becauseArgs)
        {
            var tree = CSharpSyntaxTree.ParseText(_instance);
            var expectedTree = CSharpSyntaxTree.ParseText(sourceCode);

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(tree.IsEquivalentTo(expectedTree))
                .FailWith("The syntax trees of both source codes are not the same: {0} vs {1}", _instance, sourceCode);

            return new AndConstraint<SourceCodeStringAssertions>(this);
        }
    }
}
