using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Views.MainView
{
    public interface IMainView
    {
        void PaintTable();
        void SaveExcel();
        void ShowMessag(string msg);
        void Init();
        void GetTransaction();
    }
}
