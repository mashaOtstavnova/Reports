using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Settings
    {
        public readonly string Login;
        public readonly string Password;
        public readonly string BaseAddress;
        public readonly DateTime From;
        public readonly DateTime To;
        public string IdOrganization;
        public string IdCorparate;
        public readonly int TIMEOUT;

        public Settings(string login, string pasword, string baseAddress, int timout, DateTime from, DateTime to)
        {
            Login = login;
            BaseAddress = baseAddress;
            Password = pasword;
            TIMEOUT = timout;
            From = from;
            To = to;
        }
    }
}
