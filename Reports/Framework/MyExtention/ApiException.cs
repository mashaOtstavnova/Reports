using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.MyExtention
{
    public class ApiException : Exception
    {
        public Exception Exception;
        public string Response;

        public ApiException(string response = null, string message = "", Exception ex = null) : base(message)
        {
            Exception = ex;
            Response = response;
        }
    }
}
