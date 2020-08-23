using System.Threading.Tasks;
using Dash.Engine.Abstractions;
using Dash.Extensions;
using Dash.Nodes;

namespace Dash.Engine.Visitors
{
    public class UriResourceDownload : BaseVisitor
    {
        private readonly IUriResourceRepository _uriResourceRepository;
        private readonly IDownloadHttpResource _downloadHttpResource;

        public UriResourceDownload(
            IConsole console,
            IUriResourceRepository uriResourceRepository,
            IDownloadHttpResource downloadHttpResource) : base(console)
        {
            _uriResourceRepository = uriResourceRepository;
            _downloadHttpResource = downloadHttpResource;
        }

        public override async Task Visit(UriNode node)
        {
            if (await _uriResourceRepository.Exists(node.Uri))
            {
                return;
            }

            if (node.Uri.Scheme.IsSame("https") || node.Uri.Scheme.IsSame("http"))
            {
                var downloadResult = await _downloadHttpResource.Download(node.Uri);
                if (downloadResult.Success)
                {
                    await _uriResourceRepository.Add(node.Uri, downloadResult.FileName!, downloadResult.Content!);
                }
            }
            else
            {
                await _uriResourceRepository.Add(node.Uri);
            }

            await base.Visit(node);
        }
    }
}
