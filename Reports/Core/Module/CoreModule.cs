using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Services;
using Core.Services.Implimintation;
using Framework;
using IikoBizApi;

namespace Core.Module
{
    public class CoreModule : Framework.Module
    {
        /// <summary>
        /// регистратор всех сервисов
        /// </summary>
        /// <param name="registrator"> регистратор </param>
        public override void RegisterServices(Registrator registrator)
        {
            registrator.RegistrServiceFactory<IBizApiClient>(() => new IikoBizApiClient());
            registrator.RegistrServiceFactory<IInitService>(() => new InitService());
            registrator.RegistrServiceFactory<IBuildTableAndSaveExcel>(() => new BuildTableAndSaveExcel());
            registrator.RegistrServiceFactory<IParseCorparationToReports>(() => new ParseCorparationToReports());
            registrator.RegistrServiceFactory<IMakerRequest>(() => new MakerRequest());
            registrator.RegistrServiceFactory<IViewService>(() => new ViewService());
            //registrator.RegistrServiceFactory<IBetsService>(() => new BetsService());
        }
    }
}
