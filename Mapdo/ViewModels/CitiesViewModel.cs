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
    public class CitiesViewModel : IViewModel
    {
        // ===========================================================================
        // = Public Properties
        // ===========================================================================
        
        public RealmResults<City> Cities { get; }
        public ICommand DeleteCity { get; }

        // ===========================================================================
        // = Construction
        // ===========================================================================
        
        public CitiesViewModel()
        {
            var realm = Realm.GetInstance();
            Cities = realm.All<City>();

            DeleteCity = new Command(DoDeleteCity);
        }

        // ===========================================================================
        // = Private Methods - Command Implementations
        // ===========================================================================
        
        private void DoDeleteCity(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            if (!(obj is City))
                throw new ArgumentException($"Unexpected type '{obj.GetType().Name}'. Expected {typeof(City).Name}.");

            var city = (City)obj;

            var realm = Realm.GetInstance();
            realm.Remove(city);
        }
    }
}
