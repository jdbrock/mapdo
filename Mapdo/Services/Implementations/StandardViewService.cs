using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Autofac;

namespace Mapdo.Services
{
    public class StandardViewService : IViewService
    {
        private readonly IDictionary<Type, Type> _map = new Dictionary<Type, Type>();
        private readonly IComponentContext _componentContext;

        public StandardViewService(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        public void Register<TViewModel, TView>()
            where TViewModel : class, IViewModel
            where TView : Page
        {
            _map[typeof(TViewModel)] = typeof(TView);
        }

        public ViewPage<TViewModel> Resolve<TViewModel>(Action<TViewModel> setStateAction = null) where TViewModel : class, IViewModel
        {
            TViewModel viewModel;
            return Resolve<TViewModel>(out viewModel, setStateAction);
        }

        public ViewPage<TViewModel> Resolve<TViewModel>(out TViewModel viewModel, Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            viewModel = _componentContext.Resolve<TViewModel>();

            var viewType = _map[typeof(TViewModel)];
            var view = _componentContext.Resolve(viewType) as ViewPage<TViewModel>;

            if (setStateAction != null)
                viewModel.SetState(setStateAction);

            view.ViewModel = viewModel;
            return view;
        }

        public ViewPage<TViewModel> Resolve<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var viewType = _map[typeof(TViewModel)];
            var view = _componentContext.Resolve(viewType) as ViewPage<TViewModel>;
            view.ViewModel = viewModel;
            return view;
        }
    }
}
