using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;

namespace Core.Services.Implimintation
{
    /// <summary>
    /// хранилище сервисов
    /// </summary>
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
        public static IParseCorparationToReports ParseCorparationToReports
        {
            get { return Service<IParseCorparationToReports>.GetInstance(); }
        }
        public static IMakerRequest MakerRequest
        {
            get { return Service<IMakerRequest>.GetInstance(); }
        }
        public static IViewService ViewService
        {
            get { return Service<IViewService>.GetInstance(); }
        }
    }
}