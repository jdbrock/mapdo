using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Mapdo
{
    public partial class TripView : ContentPage
    {
        // ===========================================================================
        // = Public Properties
        // ===========================================================================
        
        public TripViewModel ViewModel { get { return BindingContext as TripViewModel; } set { BindingContext = value; } }

        // ===========================================================================
        // = Construction
        // ===========================================================================
        
        public TripView(TripViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }

        // ===========================================================================
        // = Event Handling
        // ===========================================================================
        
        public void OnItemTapped(Object sender, ItemTappedEventArgs args)
        {
            var trip = (Trip)args.Item;

            var viewModel = new DashboardViewModel(trip);
            var view = new DashboardView(viewModel);

            Navigation.PushAsync(view);
        }

        public async void OnAdd(Object sender, EventArgs args)
        {
            var result = await UserDialogs.Instance.PromptAsync("Where are you going? Enter a city and state.");

            if (result.Ok)
            {
                if (ViewModel.Trips.Any(X => X.Name.Equals(result.Text, StringComparison.OrdinalIgnoreCase)))
                    return;

                using (var dialog = UserDialogs.Instance.Loading("Loading City..."))
                {
                    var trip = new Trip(result.Text);

                    var gc = new Geocoder();
                    var pos = (await gc.GetPositionsForAddressAsync(trip.Name)).FirstOrDefault();

                    if (pos != null) // TODO: error message
                    {
                        trip.Latitude = pos.Latitude;
                        trip.Longitude = pos.Longitude;
                    }

                    ViewModel.Trips.Add(trip);
                    App.Save();
                }
            }
        }

        private async void OnMenuItemDeleteClicked(object sender, EventArgs e)
        {
            var menuItem = (MenuItem)sender;
            var trip = (Trip)menuItem.BindingContext;

            var result = await DisplayAlert("Delete Trip", "Are you sure you want to remove this trip? Careful! This can't be undone.", "Delete Trip", "Cancel");

            if (!result)
                return;

            ViewModel.Trips.Remove(trip);
            App.Save();
        }
    }
}
