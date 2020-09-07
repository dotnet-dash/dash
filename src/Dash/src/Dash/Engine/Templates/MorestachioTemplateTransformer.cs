// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;

namespace Dash.Engine.Templates
{
    public class MorestachioTemplateTransformer : ITemplateTransformer
    {
        private readonly IModelRepository _modelRepository;

        public MorestachioTemplateTransformer(IModelRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public async Task<string> Transform(string template)
        {
            var options = new Morestachio.ParserOptions(template);
            var transformer = Morestachio.Parser.ParseWithOptions(options);

            return await transformer.CreateAndStringifyAsync
            (
                new
                {
                    Entities = _modelRepository.EntityModels
                }
            );
        }
    }
}
