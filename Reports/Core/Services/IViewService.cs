using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Views;
using Core.Views.FirstView;
using Core.Views.MainView;

namespace Core.Services
{
    public interface IViewService // изменение вью, отображение на форму, обратная связь с кодом
    {
        IMainView MainView { get; }
        void Init(IMainView view);//что бы получить MainView
        IFirstView FirstView { get; }
        void Init(IFirstView view);//что бы получить MainView
    }
}
