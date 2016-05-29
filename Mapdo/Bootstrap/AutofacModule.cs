using Autofac;
using Mapdo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mapdo.Bootstrap
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StandardViewService>()
                .As<IViewService>()
                .SingleInstance();

            builder.RegisterType<StandardNavigationService>()
                .As<INavigationService>()
                .SingleInstance();

            builder.Register<INavigation>(context => App.Current.MainPage.Navigation)
                .SingleInstance();
        }
    }
}
