using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mapdo
{
    public class ListView : Xamarin.Forms.ListView
    {
        public static BindableProperty ItemTappedCommandProperty = BindableProperty.Create(nameof(ItemTappedCommand), typeof(ICommand), typeof(ListView), null);
        public ICommand ItemTappedCommand
        {
            get { return (ICommand)this.GetValue(ItemTappedCommandProperty); }
            set { this.SetValue(ItemTappedCommandProperty, value); }
        }

        public ListView()
        {
            this.ItemTapped += this.OnItemTapped;
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null && this.ItemTappedCommand != null && this.ItemTappedCommand.CanExecute(e))
            {
                this.ItemTappedCommand.Execute(e.Item);
                this.SelectedItem = null;
            }
        }
    }
}
