using Acr.UserDialogs;
using Mapdo.Models;
using PropertyChanged;
using Realms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mapdo.ViewModels
{
    [ImplementPropertyChanged]
    public class CitiesViewModel : ViewModelBase
    {
        // ===========================================================================
        // = Public Properties
        // ===========================================================================
        
        public IList<City> Cities { get; private set; }
        public ICommand DeleteCity { get; }

        // ===========================================================================
        // = Construction
        // ===========================================================================
        
        public CitiesViewModel()
        {
            var realm = Realm.GetInstance();
            Cities = (IList<City>)realm.All<City>().ToNotifyCollectionChanged(error => { });

            DeleteCity = new Command(DoDeleteCity);
        }

        // ===========================================================================
        // = Private Methods - Command Implementations
        // ===========================================================================

        private async void DoDeleteCity(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            if (!(obj is City))
                throw new ArgumentException($"Unexpected type '{obj.GetType().Name}'. Expected {typeof(City).Name}.");

            var confirm = await UserDialogs.Instance.ConfirmAsync("Are you sure you want to delete this city? This can't be undone.", "Delete City", "Delete City", "Cancel");

            if (!confirm)
                return;

            var city = (City)obj;

            var realm = Realm.GetInstance();

            realm.Write(() =>
            {
                realm.Remove(city);
            });
        }
    }
}
