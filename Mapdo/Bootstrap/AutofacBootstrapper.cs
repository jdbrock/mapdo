using Autofac;
using Mapdo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapdo.Bootstrap
{
    public abstract class AutofacBootstrapper
    {
        public void Run()
        {
            var builder = new ContainerBuilder();

            ConfigureContainer(builder);

            var container = builder.Build();
            var viewService = container.Resolve<IViewService>();

            RegisterViews(viewService);

            ConfigureApplication(container);
        }

        protected virtual void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<AutofacModule>();
        }

        protected abstract void RegisterViews(IViewService viewService);

        protected abstract void ConfigureApplication(IContainer container);
    }
}
