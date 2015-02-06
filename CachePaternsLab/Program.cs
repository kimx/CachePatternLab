using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace CachePatternLab
{
    class ProgramByArrayCacheLab
    {
        const double CacheDuration = 61.0;
        private static readonly string[] MasterCacheKeyArray = { "ProductsCache" };
        static void Main(string[] args)
        {
            Console.WriteLine(GetList());
            Console.WriteLine(HttpRuntime.Cache.Count);
            Console.WriteLine(GetListById(1));
            Console.WriteLine(HttpRuntime.Cache.Count);
            InvalidateCache();
            Console.WriteLine(HttpRuntime.Cache.Count);

            Console.ReadLine();
        }

        private static string GetCacheKey(string cacheKey)
        {
            return string.Concat(MasterCacheKeyArray[0], "-", cacheKey);
        }

        private static object GetCacheItem(string rawKey)
        {
            return HttpRuntime.Cache[GetCacheKey(rawKey)];
        }


        private static void AddCacheItem(string rawKey, object value)
        {

            var cache = HttpRuntime.Cache;
            if (cache[MasterCacheKeyArray[0]] == null)//此段的作用是為了相依CacheDependency的快速鍵MasterCacheKeyArray，代表所有的子項目會相依它，若它移除，則子集合會跟著移除
                cache[MasterCacheKeyArray[0]] = DateTime.Now;
            CacheDependency dependency = new CacheDependency(null, MasterCacheKeyArray);
            cache.Insert(GetCacheKey(rawKey), value, dependency, DateTime.Now.AddSeconds(CacheDuration), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        private static string GetList()
        {
            string rawKey = "GetList";
            string result = GetCacheItem(rawKey) as string;
            if (result == null)
            {
                result = rawKey + "=" + DateTime.Now.ToString("yyyyMMddHHmmss");
                AddCacheItem(rawKey, result);
            }
            return result;
        }
        private static string GetListById(int id)
        {
            string rawKey = string.Concat("GetListById", id);
            string result = GetCacheItem(rawKey) as string;
            if (result == null)
            {
                result = rawKey + "=" + DateTime.Now.ToString("yyyyMMddHHmmss");
                AddCacheItem(rawKey, result);
            }
            return result;
        }


        public static void InvalidateCache()
        {
            HttpRuntime.Cache.Remove(MasterCacheKeyArray[0]);
        }
    }
}
