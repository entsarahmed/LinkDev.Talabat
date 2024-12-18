using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Abstraction.Common.Contracts.Infrastructure
{
    public interface IResponseCacheService
    {

        Task CacheResponseAsync(string key, object response, TimeSpan timeToLive);

        Task<string?> GetCachedResponseAsync(string key);
    }
}
