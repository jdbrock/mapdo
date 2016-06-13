using Acr.UserDialogs;
using Mapdo.Models;
using Mapdo.Services;
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
using Xamarin.Forms.Maps;

namespace Mapdo.ViewModels
{
    [ImplementPropertyChanged]
    public class CitiesViewModel : ViewModelBase
    {
        // ===========================================================================
        // = Public Properties
        // ===========================================================================

        public IList<City> Cities { get; private set; }

        public ICommand AddCityCommand { get; }
        public ICommand DeleteCityCommand { get; }
        public ICommand ShowCityCommand { get; }

        // ===========================================================================
        // = Private Fields
        // ===========================================================================

        private INavigationService _navigationService;

        // ===========================================================================
        // = Construction
        // ===========================================================================

        public CitiesViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            AddCityCommand = new Command(AddCity);
            DeleteCityCommand = new Command(DeleteCity);
            ShowCityCommand = new Command(ShowCity);

            var realm = Realm.GetInstance();
            Cities = (IList<City>)realm.All<City>().ToNotifyCollectionChanged(error => { });
        }

        // ===========================================================================
        // = Private Methods - Command Implementations
        // ===========================================================================

        private async void AddCity(object obj)
        {
            await _navigationService.PushAsync<AddCityViewModel>();
        }

        private async void DeleteCity(object obj)
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

        private async void ShowCity(object o)
        {
            var city = (City)o;

            await _navigationService.PushAsync<CityViewModel>(vm =>
            {
                var realm = Realm.GetInstance();
                var cityName = city.Name;

                vm.City = city;
            });
        }
    }
}
