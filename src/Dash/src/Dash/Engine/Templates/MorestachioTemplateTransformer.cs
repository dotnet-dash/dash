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
