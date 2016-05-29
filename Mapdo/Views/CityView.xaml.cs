//using Acr.UserDialogs;
using Brock.Services;
using Mapdo.Models;
using Mapdo.ViewModels;
using Realms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using YelpSharp;

namespace Mapdo.Views
{
    public partial class CityView : ViewPage<CityViewModel>
    {
        // ===========================================================================
        // = Construction
        // ===========================================================================
        
        public CityView(CityViewModel vm)
        {
            //ViewModel = vm;
            //ViewModel.View = this;

            InitializeComponent();

            // Sort old data.
            //ViewModel.Trip.Places = new ObservableCollection<Poi>(
            //    ViewModel.Trip.Places
            //        .OrderBy(X => X.Name));

            //App.Save();

            vm.Changed += (S, E) => RecreatePins();
            searchBar.SearchButtonPressed += OnSearchButtonPressed;
            searchBar.TextChanged += OnSearchTextChanged;

            vm.NavigationRequested += OnNavigationRequested;
        }

        // ===========================================================================
        // = Event Handling
        // ===========================================================================

        private void OnNavigationRequested(object sender, NavigationRequestedArgs e)
        {
            var page = ViewHelper.CreateView(e.ViewModel);
            Navigation.PushAsync(page);
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            ViewModel.IsPortrait = height > width;

            if (ViewModel.IsPortrait)
            {
                Grid.SetColumn(itemsGrid, 0);
                Grid.SetRow(itemsGrid, 1);

                itemsGrid.WidthRequest = 0;
                itemsGrid.HeightRequest = height / 2;
            }
            else
            {
                Grid.SetColumn(itemsGrid, 1);
                Grid.SetRow(itemsGrid, 0);

                itemsGrid.WidthRequest = width / 3;
                itemsGrid.HeightRequest = 0;
            }
        }

        protected override void OnAppearing()
        {
            var cityCenter = new Position(ViewModel.City.Latitude, ViewModel.City.Longitude);
            var citySpan = MapSpan.FromCenterAndRadius(cityCenter, Distance.FromMiles(10));

            map.MoveToRegion(citySpan);
            map.ItemsSource = ViewModel.Pins;

            RecreatePins();
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(e.NewTextValue))
            {
                ViewModel.IsSearching = false;
                ClearSearchResults();
            }
        }

        public void OnSavedItemTapped(Object sender, ItemTappedEventArgs args)
        {
            // Deselect.
            var list = (ListView)sender;
            list.SelectedItem = null;

            // Pan to place.
            var place = (Place)args.Item;
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(place.Latitude, place.Longitude), Distance.FromMiles(0.1)));
        }

        public void OnSearchResultTapped(Object sender, ItemTappedEventArgs args)
        {
            var poi = (Place)args.Item;
            poi.IsSearchResult = false;

            ViewModel.City.Places.Add(poi);

            //ViewModel.City.Places = new RealmList<Place><>(
            //    ViewModel.City.Places
            //        .OrderBy(X => X.Name));

            ViewModel.Pins.Add(CreateSavedPinFromPlace(poi));

            ViewModel.IsSearching = false;
            ClearSearchResults();
            searchBar.Text = "";

            //UserDialogs.Instance.ShowSuccess("Saved to Map");

            RefreshMapRenderer();
            App.Save();
        }

        private async void OnSearchButtonPressed(object sender, EventArgs e)
        {
            ViewModel.IsSearching = true;

            await new Task(() => { });

            //using (var dialog = UserDialogs.Instance.Loading("Searching..."))
            //{
            //    var yelpClient          = new YelpClient(App.Config.Yelp.AccessToken, App.Config.Yelp.AccessTokenSecret, App.Config.Yelp.ConsumerKey, App.Config.Yelp.ConsumerSecret);
            //    var yelpGeneralOptions  = new YelpSearchOptionsGeneral(query: searchBar.Text, radiusFilter: 25000);
            //    var yelpLocationOptions = new YelpSearchOptionsLocation(ViewModel.City.Name);
            //    var yelpSearchOptions   = new YelpSearchOptions(general: yelpGeneralOptions, location: yelpLocationOptions);

            //    var yelpResults         = await yelpClient.SearchWithOptions(yelpSearchOptions);

            //    ClearSearchResults();

            //    var currentPois = new HashSet<String>(ViewModel.City.Places
            //        .Select(X => X.Address)
            //        .Distinct());

            //    foreach (var business in yelpResults.businesses)
            //    {
            //        var place = new Place
            //        {
            //            Name = business.name,
            //            Latitude = business.location.coordinate.Latitude,
            //            Longitude = business.location.coordinate.Longitude,
            //            Address = String.Join(", ", business.location.display_address),
            //            IsSearchResult = true,
            //            //ExternalYelpData = business
            //        };

            //        if (currentPois.Contains(place.Address))
            //            continue;

            //        var pin = CreateSearchResultPinFromPlace(place);

            //        ViewModel.Pins.Add(pin);
            //        ViewModel.SearchResults.Add(place);
            //    }

            //    var positions = ViewModel.SearchResults
            //        .Select(X => new Position(X.Latitude, X.Longitude))
            //        .ToList();

            //    map.ZoomToExtent(positions);

            //    RefreshMapRenderer();
            //}
        }

        // ===========================================================================
        // = Private Methods
        // ===========================================================================

        private void RecreatePins()
        {
            ViewModel.Pins.Clear();

            foreach (var place in ViewModel.City.Places)
                ViewModel.Pins.Add(CreateSavedPinFromPlace(place));

            if (ViewModel.IsSearching)
                foreach (var place in ViewModel.SearchResults)
                    ViewModel.Pins.Add(CreateSearchResultPinFromPlace(place));

            RefreshMapRenderer();
        }

        private ExtendedPin CreateSavedPinFromPlace(Place place)
        {
            return new ExtendedPin(place.Name, place.Address, place.Latitude, place.Longitude)
            {
                PinColor = place.IsDone
                    ? StandardPinColor.Green
                    : StandardPinColor.Red,

                IsSearchResult = false
            };
        }

        private ExtendedPin CreateSearchResultPinFromPlace(Place place)
        {
            return new ExtendedPin(place.Name, place.Address, place.Latitude, place.Longitude)
            {
                PinColor = StandardPinColor.Purple,
                IsSearchResult = true
            };
        }

        private void ClearSearchResults()
        {
            // Clear search results.
            var pinsToRemove = ViewModel.Pins
                .Select((X, I) => new { Index = I, Object = X })
                .Where(X => X.Object.IsSearchResult)
                .Select(X => X.Index)
                .OrderByDescending(X => X)
                .ToList();

            foreach (var index in pinsToRemove)
                ViewModel.Pins.RemoveAt(index);

            ViewModel.SearchResults.Clear();

            if (pinsToRemove.Count > 0)
                RefreshMapRenderer();
        }

        private void RefreshMapRenderer()
        {
            map.RefreshPins();
        }
    }
}
