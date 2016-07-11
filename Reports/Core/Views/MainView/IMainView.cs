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
        void PaintTable(DataTable dt);
        void SaveExcel();
    }
}
