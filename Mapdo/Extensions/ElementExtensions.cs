using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mapdo
{
    public static class ElementExtensions
    {
        public static T FindParent<T>(this Element @this)
            where T : Element
        {
            var parent = @this.Parent;

            while (parent != null)
            {
                if (parent is T)
                    return (T)parent;

                parent = parent.Parent;
            }

            return null;
        }
    }
}
