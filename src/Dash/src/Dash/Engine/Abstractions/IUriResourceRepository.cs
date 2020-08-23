using System;
using System.Threading.Tasks;

namespace Dash.Engine.Abstractions
{
    public interface IUriResourceRepository
    {
        Task Add(Uri uriResource);

        Task Add(Uri uriResource, string fileName, byte[] contents);

        Task<string> Get(Uri uriResource);

        Task<bool> Exists(Uri uriResource);

        Task<string> GetContents(Uri uriResource);
    }
}