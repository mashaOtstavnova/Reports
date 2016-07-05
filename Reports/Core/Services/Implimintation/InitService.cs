using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;

namespace Core.Services.Implimintation
{
    public class InitService : IInitService
    {
        public void Init( Settings settings)
        {
            //restoCamp
            //9U6cZjDCG3zm9jj
            //var settings = new Settings("vankorAPI", "JUe5J4cuWu", "https://iiko.biz:9900/api/0/", 60000, new DateTime().Date, new DateTime().Date );
            CoreContext.BizApiClient.Init(settings,null);


        }
    }
}
