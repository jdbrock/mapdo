using Brock.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mapdo
{
    public static class ViewHelper
    {
        private static Dictionary<Type, Type> _viewModelToViewMappings;

        public static void InitializeMappings()
        {
            if (_viewModelToViewMappings != null)
                return;

            _viewModelToViewMappings = new Dictionary<Type, Type>();

            foreach (var assembly in Services.Assembly.GetAssemblies())
                foreach (var type in assembly.DefinedTypes)
                {
                    var attribute = type.GetCustomAttribute<ViewForAttribute>();

                    if (attribute == null)
                        continue;

                    _viewModelToViewMappings.Add(attribute.ViewModelType, type.AsType());
                }
        }

        public static Page CreateView(IViewModel viewModel)
        {
            InitializeMappings();

            var viewModelType = viewModel.GetType();
            var viewType      = _viewModelToViewMappings[viewModelType];
            var view          = System.Activator.CreateInstance(viewType, viewModel);

            return (Page)view;
        }
    }
}
