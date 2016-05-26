using Acr.UserDialogs;
using Brock.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using YelpSharp;

namespace Mapdo
{
    public partial class DashboardView : ContentPage
    {
        // ===========================================================================
        // = Public Properties
        // ===========================================================================
        
        public DashboardViewModel ViewModel { get { return BindingContext as DashboardViewModel; } set { BindingContext = value; } }

        // ===========================================================================
        // = Construction
        // ===========================================================================
        
        public DashboardView(DashboardViewModel vm)
        {
            ViewModel = vm;
            ViewModel.View = this;

            InitializeComponent();

            // Sort old data.
            //ViewModel.Trip.POIs = new ObservableCollection<Poi>(
            //    ViewModel.Trip.POIs
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
            var cityCenter = new Position(ViewModel.Trip.Latitude, ViewModel.Trip.Longitude);
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

            // Pan to POI.
            var poi = (Poi)args.Item;
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(poi.Latitude, poi.Longitude), Distance.FromMiles(0.1)));
        }

        public void OnSearchResultTapped(Object sender, ItemTappedEventArgs args)
        {
            var poi = (Poi)args.Item;
            poi.IsSearchResult = false;

            ViewModel.Trip.POIs.Add(poi);

            ViewModel.Trip.POIs = new ObservableCollection<Poi>(
                ViewModel.Trip.POIs
                    .OrderBy(X => X.Name));

            ViewModel.Pins.Add(CreateSavedPinFromPoi(poi));

            ViewModel.IsSearching = false;
            ClearSearchResults();
            searchBar.Text = "";

            UserDialogs.Instance.ShowSuccess("Saved to Map");

            RefreshMapRenderer();
            App.Save();
        }

        private async void OnSearchButtonPressed(object sender, EventArgs e)
        {
            ViewModel.IsSearching = true;

            using (var dialog = UserDialogs.Instance.Loading("Searching..."))
            {
                var yelpClient = new YelpClient(App.Config.Yelp.AccessToken, App.Config.Yelp.AccessTokenSecret, App.Config.Yelp.ConsumerKey, App.Config.Yelp.ConsumerSecret);

                var yelpGeneralOptions  = new YelpSearchOptionsGeneral(query: searchBar.Text, radiusFilter: 25000);
                var yelpLocationOptions = new YelpSearchOptionsLocation(ViewModel.Trip.Name);
                var yelpSearchOptions   = new YelpSearchOptions(general: yelpGeneralOptions, location: yelpLocationOptions);

                var yelpResults         = await yelpClient.SearchWithOptions(yelpSearchOptions);

                ClearSearchResults();

                var currentPois = new HashSet<String>(ViewModel.Trip.POIs
                    .Select(X => X.Address)
                    .Distinct());

                foreach (var x in yelpResults.businesses)
                {
                    var poi = new Poi(x.name, x.location.coordinate.Latitude, x.location.coordinate.Longitude)
                    {
                        Address = String.Join(", ", x.location.display_address),
                        IsSearchResult = true,
                        ExternalYelpData = x
                    };

                    if (currentPois.Contains(poi.Address))
                        continue;

                    var pin = CreateSearchResultPinFromPoi(poi);

                    ViewModel.Pins.Add(pin);
                    ViewModel.SearchResults.Add(poi);
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

            foreach (var poi in ViewModel.Trip.POIs)
                ViewModel.Pins.Add(CreateSavedPinFromPoi(poi));

            if (ViewModel.IsSearching)
                foreach (var poi in ViewModel.SearchResults)
                    ViewModel.Pins.Add(CreateSearchResultPinFromPoi(poi));

            RefreshMapRenderer();
        }

        private ExtendedPin CreateSavedPinFromPoi(Poi poi)
        {
            return new ExtendedPin(poi.Name, poi.Address, poi.Latitude, poi.Longitude)
            {
                PinColor = poi.IsDone
                    ? StandardPinColor.Green
                    : StandardPinColor.Red,

                IsSearchResult = false
            };
        }

        private ExtendedPin CreateSearchResultPinFromPoi(Poi poi)
        {
            return new ExtendedPin(poi.Name, poi.Address, poi.Latitude, poi.Longitude)
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
