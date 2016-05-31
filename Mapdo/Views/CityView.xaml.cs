using Acr.UserDialogs;
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
    public class CityViewBase : ViewPage<CityViewModel> { }
    public partial class CityView : CityViewBase
    {
        // ===========================================================================
        // = Private Fields
        // ===========================================================================

        private double? _lastWidthAllocated;
        private double? _lastHeightAllocated;

        // ===========================================================================
        // = Construction
        // ===========================================================================

        public CityView()
        {
            InitializeComponent();

            searchBar.SearchButtonPressed += OnSearchButtonPressed;
            searchBar.TextChanged += OnSearchTextChanged;
        }
        // ===========================================================================
        // = Event Handling
        // ===========================================================================

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            _lastHeightAllocated = height;
            _lastWidthAllocated = width;

            RefreshRotation();
        }

        protected override void OnAppearing()
        {
            RefreshMapExtent();
        }

        public override void OnViewModelReplaced(IViewModel oldViewModel, IViewModel newViewModel)
        {
            var oldCityViewModel = (CityViewModel)oldViewModel;
            var newCityViewModel = (CityViewModel)newViewModel;

            if (oldCityViewModel != null)
                oldCityViewModel.Changed -= OnViewModelChanged;

            if (newCityViewModel != null)
                newCityViewModel.Changed += OnViewModelChanged;
        }

        private void OnViewModelChanged(object sender, EventArgs e)
        {
            RecreatePins();
        }

        public override void OnViewModelRefreshed(object sender, EventArgs args)
        {
            Refresh();
        }

        private void Refresh()
        {
            RefreshRotation();
            RefreshMapExtent();
        }

        private void RefreshRotation()
        {
            if (!_lastHeightAllocated.HasValue || !_lastWidthAllocated.HasValue || ViewModel == null)
                return;

            var viewModel = ViewModel;
            var width  = _lastWidthAllocated.Value;
            var height = _lastHeightAllocated.Value;

            if (height > width) // Portrait
            {
                Grid.SetColumn(itemsGrid, 0);
                Grid.SetRow(itemsGrid, 1);

                itemsGrid.WidthRequest = 0;
                itemsGrid.HeightRequest = height / 2;
            }
            else // Landscape
            {
                Grid.SetColumn(itemsGrid, 1);
                Grid.SetRow(itemsGrid, 0);

                itemsGrid.WidthRequest = width / 3;
                itemsGrid.HeightRequest = 0;
            }
        }

        private void RefreshMapExtent()
        {
            if (ViewModel == null)
                return;

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
            try
            {
                var result = (SearchResult)args.Item;

                Place place = null;

                var realm = Realm.GetInstance();
                realm.Write(() =>
                {
                    // TODO: AutoMapper.
                    place = realm.CreateObject<Place>();
                    place.Address   = result.Address;
                    place.Latitude  = result.Latitude;
                    place.Longitude = result.Longitude;
                    place.Name      = result.Name;
                    place.IsDone    = false;
                    //place.Parent    = ViewModel.City.Name;

                    ViewModel.City.Places.Add(place);
                });

                if (place == null)
                    throw new Exception($"Failed to write '{typeof(Place).Name}' instance.");

                ViewModel.Pins.Add(CreateSavedPinFromPlace(place));

                UserDialogs.Instance.ShowSuccess("Saved to Map");
            }
            finally
            {
                ViewModel.IsSearching = false;

                searchBar.Text = String.Empty;
                ClearSearchResults();

                RefreshMapRenderer();
            }
        }

        private async void OnSearchButtonPressed(object sender, EventArgs e)
        {
            ViewModel.IsSearching = true;

            using (var dialog = UserDialogs.Instance.Loading("Searching..."))
            {
                var yelpClient = new YelpClient(App.Config.Yelp.AccessToken, App.Config.Yelp.AccessTokenSecret, App.Config.Yelp.ConsumerKey, App.Config.Yelp.ConsumerSecret);
                var yelpGeneralOptions = new YelpSearchOptionsGeneral(query: searchBar.Text, radiusFilter: 25000);
                var yelpLocationOptions = new YelpSearchOptionsLocation(ViewModel.City.Name);
                var yelpSearchOptions = new YelpSearchOptions(general: yelpGeneralOptions, location: yelpLocationOptions);

                var yelpResults = await yelpClient.SearchWithOptions(yelpSearchOptions);

                ClearSearchResults();

                var currentPois = new HashSet<String>(ViewModel.City.Places
                    .Select(X => X.Address)
                    .Distinct());

                foreach (var business in yelpResults.businesses)
                {
                    var result = new SearchResult
                    {
                        Name = business.name,
                        Latitude = business.location.coordinate.Latitude,
                        Longitude = business.location.coordinate.Longitude,
                        Address = String.Join(", ", business.location.display_address),
                        //ExternalYelpData = business
                    };

                    if (currentPois.Contains(result.Address))
                        continue;

                    var pin = CreateSearchResultPinFromPlace(result);

                    ViewModel.Pins.Add(pin);
                    ViewModel.SearchResults.Add(result);
                }

                var positions = ViewModel.SearchResults
                    .Select(X => new Position(X.Latitude, X.Longitude))
                    .ToList();

                map.ZoomToExtent(positions);

                RefreshMapRenderer();
            }
        }

        // ===========================================================================
        // = Private Methods
        // ===========================================================================

        private void RecreatePins()
        {
            ViewModel.Pins.Clear();

            if (ViewModel.City.Places != null)
                foreach (var place in ViewModel.City.Places)
                    ViewModel.Pins.Add(CreateSavedPinFromPlace(place));

            if (ViewModel.IsSearching)
                foreach (var result in ViewModel.SearchResults)
                    ViewModel.Pins.Add(CreateSearchResultPinFromPlace(result));

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

        private ExtendedPin CreateSearchResultPinFromPlace(SearchResult result)
        {
            return new ExtendedPin(result.Name, result.Address, result.Latitude, result.Longitude)
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
