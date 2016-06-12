
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Mapdo.Models;
using Realms;
using PropertyChanged;
using Mapdo.Services;
using Acr.UserDialogs;
using System.ComponentModel;
using YelpSharp;
using Xamarin.Forms.Maps;

namespace Mapdo.ViewModels
{
    [ImplementPropertyChanged]
    public class CityViewModel : ViewModelBase
    {
        // ===========================================================================
        // = Public Properties
        // ===========================================================================
        
        public City City { get; set; }

        public ObservableCollection<SearchResult> SearchResults { get; set; }
        public ObservableCollection<ExtendedPin> Pins { get; set; }

        public string SearchText { get; set; }

        public Boolean IsSearching { get; set; }

        public ICommand SearchCommand { get; }
        public ICommand SavedPlaceDoneCommand { get; }
        public ICommand SavedPlaceMoreCommand { get; }
        public ICommand SavedPlaceTappedCommand { get; }
        public ICommand SearchResultPlaceTappedCommand { get; }

        public event EventHandler Changed;
        public event EventHandler<Place> RequestMapNavigation;
        public event EventHandler<Position[]> RequestMapNavigationExtent;

        // ===========================================================================
        // = Private Fields
        // ===========================================================================

        private INavigationService _navigationService;

        // ===========================================================================
        // = Construction
        // ===========================================================================

        public CityViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            SearchResults = new ObservableCollection<SearchResult>();
            Pins = new ObservableCollection<ExtendedPin>();

            SearchCommand = new Command(OnSearch);
            SavedPlaceDoneCommand = new Command(OnSavedPlaceDone);
            SavedPlaceMoreCommand = new Command(OnSavedPlaceMore);
            SavedPlaceTappedCommand = new Command(OnSavedPlaceTapped);
            SearchResultPlaceTappedCommand = new Command(OnSearchResultPlaceTapped);

            ((INotifyPropertyChanged)this).PropertyChanged += OnPropertyChangedCore;
        }

        // ===========================================================================
        // = Public Methods
        // ===========================================================================

        public void ClearSearchResults()
        {
            // Clear search results.
            var pinsToRemove = Pins
                .Select((X, I) => new { Index = I, Object = X })
                .Where(X => X.Object.IsSearchResult)
                .Select(X => X.Index)
                .OrderByDescending(X => X)
                .ToList();

            foreach (var index in pinsToRemove)
                Pins.RemoveAt(index);

            SearchResults.Clear();

            if (pinsToRemove.Count > 0)
                RaiseChanged();
        }

        // ===========================================================================
        // = Private Methods
        // ===========================================================================

        private void RaiseChanged()
        {
            if (Changed != null)
                Changed.Invoke(this, EventArgs.Empty);
        }

        private void RaiseMapNavigation(Place place)
        {
            if (RequestMapNavigation != null)
                RequestMapNavigation(this, place);
        }

        public void HACK_ResetCityToTriggerRefresh()
        {
            var x = City;
            City = null;
            City = x;
        }

        // ===========================================================================
        // = Private Methods - Commands
        // ===========================================================================

        private void OnSavedPlaceDone(Object obj)
        {
            var item = obj as Place;

            var realm = Realm.GetInstance();
            realm.Write(() =>
            {
                item.IsDone = !item.IsDone;
            });

            RaiseChanged();
        }

        private async void OnSavedPlaceMore(Object obj)
        {
            var item = obj as Place;
            var result = await Acr.UserDialogs.UserDialogs.Instance.ActionSheetAsync(item.Name, "Cancel", "Delete Place", null, "Show Info", "View in Yelp", "Book an Uber");
            var encode = (Func<String, String>)WebUtility.UrlEncode;

            switch (result)
            {
                case "Cancel":
                    break;

                case "Delete Place":
                {
                    var realm = Realm.GetInstance();
                    realm.Write(() =>
                    {
                        realm.Remove(item);
                    });

                    RaiseChanged();
                    HACK_ResetCityToTriggerRefresh();
                    break;
                }

                // TODO: Reinstate Show Info
                case "Show Info":
                {
                    if (!item.HasYelpData)
                        UserDialogs.Instance.ShowError("Yelp data missing.");
                    else
                        await _navigationService.PushAsync<PlaceViewModel>(X => X.Place = item);

                    break;
                }

                case "Book an Uber":
                {
                    var uri = new Uri($"uber://?action=setPickup&pickup=my_location&client_id=Mapdo"
                        + $"&dropoff[latitude]={item.Latitude}&dropoff[longitude]={item.Longitude}"
                        + $"&dropoff[nickname]={encode(item.Name)}&dropoff[formatted_address]={encode(item.Address)}");
                    Device.OpenUri(uri);
                    break;
                }

                case "View in Yelp":
                {
                    if (!item.HasYelpData)
                        UserDialogs.Instance.ShowError("Yelp data missing.");
                    else
                    {
                        var uri = new Uri($"yelp:///biz/{item.YelpId}");
                        Device.OpenUri(uri);
                    }
                    break;
                }
            }
        }

        private void OnSavedPlaceTapped(object place)
        {
            RaiseMapNavigation((Place)place);
        }

        private async void OnSearch(object obj)
        {
            IsSearching = true;

            using (var dialog = UserDialogs.Instance.Loading("Searching..."))
            {
                var yelpClient = new YelpClient(App.Config.Yelp.AccessToken, App.Config.Yelp.AccessTokenSecret, App.Config.Yelp.ConsumerKey, App.Config.Yelp.ConsumerSecret);
                var yelpGeneralOptions = new YelpSearchOptionsGeneral(query: SearchText, radiusFilter: 25000);
                var yelpLocationOptions = new YelpSearchOptionsLocation(City.Name);
                var yelpSearchOptions = new YelpSearchOptions(general: yelpGeneralOptions, location: yelpLocationOptions);

                var yelpResults = await yelpClient.SearchWithOptions(yelpSearchOptions);

                ClearSearchResults();

                var currentPois = new HashSet<String>(City.Places
                    .Select(X => X.Address)
                    .Distinct());

                foreach (var business in yelpResults.businesses)
                {
                    if (business?.location?.coordinate == null)
                        continue; // Ignore businesses without a location.

                    var result = new SearchResult
                    {
                        Name = business.name,
                        Latitude = business.location.coordinate.Latitude,
                        Longitude = business.location.coordinate.Longitude,
                        Address = String.Join(", ", business.location.display_address),
                        YelpData = business
                    };

                    if (currentPois.Contains(result.Address))
                        continue;

                    SearchResults.Add(result);
                }

                var positions = SearchResults
                    .Select(X => new Position(X.Latitude, X.Longitude))
                    .ToArray();

                RaiseChanged();
                RequestMapNavigationExtent(this, positions);
            }
        }

        private void OnSearchResultPlaceTapped(object searchResultArg)
        {
            try
            {
                Place place = null;
                SearchResult searchResult = (SearchResult)searchResultArg;

                var realm = Realm.GetInstance();
                realm.Write(() =>
                {
                    // TODO: AutoMapper.
                    place = realm.CreateObject<Place>();
                    place.Address   = searchResult.Address;
                    place.Latitude  = searchResult.Latitude;
                    place.Longitude = searchResult.Longitude;
                    place.Name      = searchResult.Name;
                    place.IsDone    = false;
                    place.SetYelpData(searchResult.YelpData);

                    City.Places.Add(place);

                    HACK_ResetCityToTriggerRefresh();
                });

                if (place == null)
                    throw new Exception($"Failed to write '{typeof(Place).Name}' instance.");

                UserDialogs.Instance.ShowSuccess("Saved to Map");
            }
            finally
            {
                IsSearching = false;

                SearchText = String.Empty;
                ClearSearchResults();

                RaiseChanged();
            }
        }

        private void OnPropertyChangedCore(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SearchText))
                OnSearchTextChanged(SearchText);
        }

        private void OnSearchTextChanged(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                IsSearching = false;
                ClearSearchResults();
            }
        }
    }
}
