using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{

    public static class Bootstrapper
    {
        private static Registrator _registrator;
        internal static Func<T> GetServiceFactory<T> () where T:class// internal что бы видеть его только в этом проекте
        {//чтобы получить фабрику нужно провести регистрацию
            //лямбда выражения
            //регистратор будет хранить функцию которая хранить ..
            return () => { return _registrator.GetService(typeof (T)) as T; };
        }

        public static void Start(params Module [] modules)//точка входа в проект
        {
            var registrator = new Registrator();
            foreach (var module in modules)
            {
                module.RegisterServices(registrator);//регистрируем
            }
            _registrator = registrator;
        }
    }
}
