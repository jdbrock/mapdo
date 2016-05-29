using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapdo.ViewModels
{
    public abstract class ViewModelBase : IViewModel
    {
        public event EventHandler StateChanged;

        public void SetState<T>(Action<T> action) where T : class, IViewModel
        {
            action(this as T);
            StateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
