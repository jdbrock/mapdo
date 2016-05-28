using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mapdo
{
    public class ViewPage<T> : ContentPage where T : IViewModel
    {
        public T ViewModel { get; }

        public ViewPage()
        {
            using (var scope = AppContainer.Container.BeginLifetimeScope())
                ViewModel = AppContainer.Container.Resolve<T>();

            BindingContext = ViewModel;
        }
    }
}
