using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Mapdo.ViewModels;

namespace Mapdo
{
    public class AppSetup
    {
        public IContainer CreateContainer()
        {
            var containerBuilder = new ContainerBuilder();
            RegisterDependencies(containerBuilder);
            return containerBuilder.Build();
        }

        protected virtual void RegisterDependencies(ContainerBuilder cb)
        {
            cb.RegisterType<CitiesViewModel>().SingleInstance();
            cb.RegisterType<CityViewModel>();
            cb.RegisterType<PlaceViewModel>();
        }
    }
}
