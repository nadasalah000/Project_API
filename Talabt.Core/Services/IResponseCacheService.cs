﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabt.Core.Services
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string CacheKey, object Response, TimeSpan ExpireTime);
        Task<string?> GetCachedResponse(string CacheKey);
    }
}
