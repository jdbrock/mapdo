using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mapdo
{
    public class ViewForAttribute : Attribute
    {
        public Type ViewModelType { get; private set; }

        public ViewForAttribute(Type viewModelType)
        {
            ViewModelType = viewModelType;
        }
    }
}
