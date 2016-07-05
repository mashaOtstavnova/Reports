using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class Registrator
    {
        private Dictionary<Type, Func<object>> _factories = new Dictionary<Type, Func<object>>() ; //регистрируем сервис
        public object Factories { get; set; }

        internal object GetService(Type type)
        {
            return _factories[type]();//вызвали функцию которая взяла из словаря по типу
        }

        public void RegistrServiceFactory<T>(Func<T> factory) where T:class 
        {
            _factories[typeof(T)] = () => factory();
        }
    }
}
