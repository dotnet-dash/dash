// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using Scriban;
using Scriban.Runtime;

namespace Dash.Engine.TemplateTransformers.Scriban
{
    public class ScribanTemplateTransformer : ITemplateTransformer
    {
        private readonly IModelRepository _modelRepository;

        public ScribanTemplateTransformer(IModelRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public async Task<string> Transform(string templateText)
        {
            var template = Template.Parse(templateText);

            var scriptObject = new ScriptObject();
            scriptObject.Add("entities", _modelRepository.EntityModels.Where(e => e.CodeGeneration));
            scriptObject.Import(typeof(GetCSharpLiteralFormatter));

            var context = new TemplateContext();
            context.PushGlobal(scriptObject);

            return await template.RenderAsync(context);
        }
    }
}
