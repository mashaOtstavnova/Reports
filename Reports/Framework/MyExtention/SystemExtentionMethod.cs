using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public static class SystemExtentionMethod
    {
        public static void Replace<T>(this List<T> self, T oldValue, T newValue)
        {
            self[self.IndexOf(oldValue)] = newValue;
        }
       
    }
}
