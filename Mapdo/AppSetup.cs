//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Autofac;
//using Mapdo.ViewModels;
//using Mapdo.Services;
//using Mapdo.Views;

//namespace Mapdo
//{
//    public class AppSetup
//    {
//        public IContainer CreateContainer()
//        {
//            var containerBuilder = new ContainerBuilder();
//            RegisterDependencies(containerBuilder);
//            return containerBuilder.Build();
//        }

//        protected virtual void RegisterDependencies(ContainerBuilder cb)
//        {
//            cb.RegisterType<CitiesViewModel>().SingleInstance();
//            cb.RegisterType<CityViewModel>();
//            cb.RegisterType<PlaceViewModel>();

//            var viewService = new StandardViewService();
//            viewService.Register<CitiesViewModel, CitiesView>();
//            viewService.Register<CityViewModel, CityView>();
//            viewService.Register<PlaceViewModel, PlaceView>();

//            cb.RegisterType<StandardViewService>()
//                .As<IViewService>()
//                .SingleInstance();
//        }
//    }
//}
