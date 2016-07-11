using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Views;
using Core.Views.FirstView;
using Core.Views.MainView;

namespace Core.Services.Implimintation
{
    public class ViewService: IViewService
    {
        public IMainView MainView { get; private set; }

        public void Init(IMainView view)
        {
            MainView = view;
        }
        public IFirstView FirstView { get; private set; }
        public void Init(IFirstView view)
        {
            FirstView = view;
        }
    }
}
