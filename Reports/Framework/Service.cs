using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public static  class Service <T> where T : class //джинерик
    {
        //пока не вызовем Service этот класс не создается
        private static readonly T _instance; //экземпляр этого класса, синглтон(единственный во всем проекте)
        public static T GetInstance()
        {
            return _instance;
        }

        static Service()// тот код который вызовется при вызове этого класса
        {
            //создание переменной
            _instance = Bootstrapper.GetServiceFactory<T>()();//взять фабрику сервис, это вернет функцию, которая создать 
        } 

    }

}
