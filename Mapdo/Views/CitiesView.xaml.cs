using Acr.UserDialogs;
using Mapdo.Models;
using Mapdo.Services;
using Mapdo.ViewModels;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Mapdo.Views
{
    public class CitiesViewBase : ViewPage<CitiesViewModel> { }
    public partial class CitiesView : CitiesViewBase
    {
        // ===========================================================================
        // = Private Fields
        // ===========================================================================

        private INavigationService _navigation;

        // ===========================================================================
        // = Construction
        // ===========================================================================

        public CitiesView(INavigationService navigation)
        {
            _navigation = navigation;

            InitializeComponent();
        }

        // ===========================================================================
        // = Event Handling
        // ===========================================================================
        
        public void OnCityTapped(Object sender, ItemTappedEventArgs args)
        {
            var city = (City)args.Item;

            _navigation.PushAsync<CityViewModel>(vm =>
            {
                var realm = Realm.GetInstance();
                var cityName = city.Name;

                vm.City = city;
            });
        }

        public async void OnAddCity(Object sender, EventArgs args)
        {
            var realm = Realm.GetInstance();
            var prompt = await UserDialogs.Instance.PromptAsync("Where are you going?");

            if (prompt.Ok)
            {
                var cityName = prompt.Text;

                if (realm.All<City>().Any(X => X.Name == cityName))
                    return;

                using (var dialog = UserDialogs.Instance.Loading("Loading..."))
                {
                    if (String.IsNullOrWhiteSpace(cityName))
                        return;

                    var geocoder = new Geocoder();
                    var position = (await geocoder.GetPositionsForAddressAsync(prompt.Text)).FirstOrDefault();

                    if (position == null)
                        return;

                    realm.Write(() =>
                    {
                        var city = realm.CreateObject<City>();

                        city.Name = cityName;
                        city.Latitude = position.Latitude;
                        city.Longitude = position.Longitude;
                    });
                }
            }
        }
    }
}
