using Acr.UserDialogs;
using Mapdo.Models;
using Mapdo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Mapdo.Views
{
    public partial class CitiesView : ViewPage<CitiesViewModel>
    {
        // ===========================================================================
        // = Construction
        // ===========================================================================
        
        public CitiesView(CitiesViewModel viewModel)
        {
            InitializeComponent();
        }

        // ===========================================================================
        // = Event Handling
        // ===========================================================================
        
        public void OnItemTapped(Object sender, ItemTappedEventArgs args)
        {
            var city = (City)args.Item;

            var viewModel = new CityViewModel(city);
            var view = new CityView(viewModel);

            Navigation.PushAsync(view);
        }

        public async void OnAdd(Object sender, EventArgs args)
        {
            var result = await UserDialogs.Instance.PromptAsync("Where are you going? Enter a city and state.");

            if (result.Ok)
            {
                if (ViewModel.Cities.Any(X => X.Name.Equals(result.Text, StringComparison.OrdinalIgnoreCase)))
                    return;

                using (var dialog = UserDialogs.Instance.Loading("Loading City..."))
                {
                    var trip = new City
                    {
                        Name = result.Text
                    };

                    var gc = new Geocoder();
                    var pos = (await gc.GetPositionsForAddressAsync(trip.Name)).FirstOrDefault();

                    if (pos != null) // TODO: error message
                    {
                        trip.Latitude = pos.Latitude;
                        trip.Longitude = pos.Longitude;
                    }

                    ViewModel.Cities.Add(trip);
                    App.Save();
                }
            }
        }

        private async void OnMenuItemDeleteClicked(object sender, EventArgs e)
        {
            var menuItem = (MenuItem)sender;
            var city = (City)menuItem.BindingContext;

            var result = await DisplayAlert("Delete City", "Are you sure you want to remove this city? Careful! This can't be undone.", "Delete City", "Cancel");

            if (!result)
                return;

            ViewModel.Cities.Remove(city);
            App.Save();
        }
    }
}
