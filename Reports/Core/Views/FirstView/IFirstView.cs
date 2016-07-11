using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Views.FirstView
{
    public interface IFirstView
    {
        void GetReports();
        void ShowMessag(string msg);
        void Init();
    }
}
