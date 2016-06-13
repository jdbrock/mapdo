using Autofac;
using Mapdo.Services;
using Mapdo.ViewModels;
using Mapdo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mapdo.Bootstrap
{
    public class AppBootstrapper : AutofacBootstrapper
    {
        private readonly App _application;

        public AppBootstrapper(App application)
        {
            _application = application;
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);
            builder.RegisterModule<AppModule>();
        }

        protected override void RegisterViews(IViewService viewService)
        {
            viewService.Register<CitiesViewModel, CitiesView>();
            viewService.Register<CityViewModel, CityView>();
            viewService.Register<PlaceViewModel, PlaceView>();
            viewService.Register<AddCityViewModel, AddCityView>();
        }

        protected override void ConfigureApplication(IContainer container)
        {
            // Set main page.
            var viewService    = container.Resolve<IViewService>();
            var mainPage       = viewService.Resolve<CitiesViewModel>();
            var navigationPage = new NavigationPage(mainPage)
            {
                BarTextColor = Color.White
            };

            _application.MainPage = navigationPage;
        }
    }
}
