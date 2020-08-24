// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using Dash.Common;
using Dash.Extensions;
using Dash.Nodes;

namespace Dash.Engine.Visitors
{
    public class UriResourceDownload : BaseVisitor
    {
        private readonly IUriResourceRepository _uriResourceRepository;
        private readonly IHttpUriDownloader _httpUriDownloader;

        public UriResourceDownload(
            IConsole console,
            IUriResourceRepository uriResourceRepository,
            IHttpUriDownloader httpUriDownloader) : base(console)
        {
            _uriResourceRepository = uriResourceRepository;
            _httpUriDownloader = httpUriDownloader;
        }

        public override async Task Visit(UriNode node)
        {
            if (!await _uriResourceRepository.Exists(node.Uri))
            {
                if (node.Uri.Scheme.IsSame("https") || node.Uri.Scheme.IsSame("http"))
                {
                    var downloadResult = await _httpUriDownloader.Download(node.Uri);
                    if (downloadResult.Success)
                    {
                        await _uriResourceRepository.Add(node.Uri, downloadResult.FileName!, downloadResult.Content!);
                    }
                }
                else
                {
                    await _uriResourceRepository.Add(node.Uri);
                }
            }

            await base.Visit(node);
        }
    }
}
