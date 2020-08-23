using System;
using System.Threading.Tasks;

namespace Dash.Engine.Abstractions
{
    public interface IUriResourceRepository
    {
        Task Add(Uri uri);

        Task Add(Uri uri, string fileName, byte[] contents);

        Task<string> Get(Uri uriResource);

        Task<bool> Exists(Uri uriResource);

        Task<string> GetContents(Uri uriResource);

        Task<int> Count();
    }
}