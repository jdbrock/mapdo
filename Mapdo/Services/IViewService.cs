using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mapdo.Services
{
    public interface IViewService
    {
        void Register<TViewModel, TView>()
            where TViewModel : class, IViewModel
            where TView : Page;

        ViewPage<TViewModel> Resolve<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel;

        ViewPage<TViewModel> Resolve<TViewModel>(out TViewModel viewModel, Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel;

        ViewPage<TViewModel> Resolve<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel;
    }
}
