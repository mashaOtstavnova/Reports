using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public static class SystemExtensionMethon
    {
        public static void Replase<T>(this List<T> self, T oldValue, T newValue)
        {
            self[self.IndexOf(oldValue)] = newValue;
        }

        public static void AddRange<K, V>(this Dictionary<K, V> dictionary, IEnumerable<K> key, Func<K, V> value)
        {
            foreach (var i in key)
            {
                dictionary.Add(i,value(i));
            }
        }
    }
}
