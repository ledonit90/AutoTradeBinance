using System;
using ServiceStack.Caching;

namespace Remibit.CachesServices.Helper
{
    public interface IRemibitMemoryCache : ICacheClientExtended, IRemoveByPattern
    {
        object Get(string key);
    }
}
