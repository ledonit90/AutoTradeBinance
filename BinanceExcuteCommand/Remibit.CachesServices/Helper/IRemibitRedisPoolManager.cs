using System.Collections.Generic;
using ServiceStack.Redis;

namespace Remibit.CachesServices.Helper
{
    public interface IRemibitRedisPoolManager
    {
        List<IRedisClientsManager> GetPools();
    }
}
