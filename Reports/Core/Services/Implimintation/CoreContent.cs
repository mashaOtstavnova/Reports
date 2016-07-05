using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;

namespace Core.Services.Implimintation
{
    public static class CoreContext
    {
        public static IBizApiClient BizApiClient
        {
            get { return Service<IBizApiClient>.GetInstance(); }
        }
        public static IInitService InitService
        {
            get { return Service<IInitService>.GetInstance(); }
        }
        public static IBuildTable BuildTable
        {
            get { return Service<IBuildTable>.GetInstance(); }
        }
    }
}