using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace O2O.Common
{
    public class CacheHelper
    {
        private const int _mins = 5;

        public static void Set(string key, object objObject) => MemoryCache.Default.Set(key, objObject, new DateTimeOffset(DateTime.Now.AddMinutes(5.0)), (string)null);

        public static object Get(string key) => MemoryCache.Default.Get(key, (string)null);

        public static List<string> GetAllKeys()
        {
            IEnumerable<KeyValuePair<string, object>> keyValuePairs = MemoryCache.Default.AsEnumerable<KeyValuePair<string, object>>();
            List<string> stringList = new List<string>();
            foreach (KeyValuePair<string, object> keyValuePair in keyValuePairs)
                stringList.Add(keyValuePair.Key);
            return stringList;
        }

        public static int GetContainsKeyCount(string str) => string.IsNullOrWhiteSpace(str) ? 0 : MemoryCache.Default.Where<KeyValuePair<string, object>>((Func<KeyValuePair<string, object>, bool>)(a => a.Key.Contains(str))).Count<KeyValuePair<string, object>>();

        public static void Remove(string key) => MemoryCache.Default.Remove(key, (string)null);
    }
}
