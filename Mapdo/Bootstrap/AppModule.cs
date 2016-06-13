using Autofac;
using Mapdo.ViewModels;
using Mapdo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapdo.Bootstrap
{
    public class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CitiesViewModel>().SingleInstance();
            builder.RegisterType<CityViewModel>().SingleInstance();
            builder.RegisterType<PlaceViewModel>().SingleInstance();
            builder.RegisterType<AddCityViewModel>().SingleInstance();

            builder.RegisterType<CitiesView>().SingleInstance();
            builder.RegisterType<CityView>().SingleInstance();
            builder.RegisterType<PlaceView>().SingleInstance();
            builder.RegisterType<AddCityView>().SingleInstance();
        }
    }
}
