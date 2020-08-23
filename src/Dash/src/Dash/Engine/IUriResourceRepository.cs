using System;
using System.Threading.Tasks;

namespace Dash.Engine
{
    public interface IUriResourceRepository
    {
        Task Add(Uri uri);

        Task Add(Uri uri, string fileName, byte[] contents);

        Task<string> Get(Uri uri);

        Task<bool> Exists(Uri uri);

        Task<string> GetContents(Uri uri);

        Task<int> Count();
    }
}