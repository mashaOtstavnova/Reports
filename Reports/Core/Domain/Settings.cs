using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    /// <summary>
    /// настройки соединения
    /// </summary>
    public class Settings
    {
        public readonly string Login;
        public readonly string Password;
        public readonly string BaseAddress;
        public readonly int TIMEOUT;

        public Settings()
        {
            //new Settings("vankorAPI", "JUe5J4cuWu", "https://iiko.biz:9900/api/0/", 60000);
            Login = "vankorAPI";
            BaseAddress = "https://iiko.biz:9900/api/0/";
            Password = "JUe5J4cuWu";
            TIMEOUT = 60000;
        }

        public Settings(string login, string pasword, string baseAddress, int timout)
        {
            Login = login;
            BaseAddress = baseAddress;
            Password = pasword;
            TIMEOUT = timout;
        }
    }
}
