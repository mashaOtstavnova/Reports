using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IBuildTable
    {
        DataTable BuiltTable(object[] obj);
        void SaveExel(DataTable dt, string path);
    }
}
