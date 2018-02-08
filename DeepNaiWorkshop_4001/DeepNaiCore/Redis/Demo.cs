using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepNaiCore.Redis
{
    class Demo
    {
        public void demo()
        {
            ICache redis = Cache.GetCache();
            String redisKey = "CURRENT_FTXIA_TAOKE_DETAIL_TEMP";
            String currentOrder = (String)redis.Get(redisKey);
        }
    }
}
