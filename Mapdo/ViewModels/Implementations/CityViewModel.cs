
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

        public Boolean IsSearching { get; set; }
        public ICommand OnItemDoneCommand { get; set; }
        public ICommand OnItemMoreCommand { get; set; }

        public event EventHandler Changed;

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

            OnItemDoneCommand = new Command(OnItemDone);
            OnItemMoreCommand = new Command(OnItemMore);
        }

        // ===========================================================================
        // = Commands
        // ===========================================================================

        private void OnItemDone(Object obj)
        {
            var item = obj as Place;

            var realm = Realm.GetInstance();
            realm.Write(() =>
            {
                item.IsDone = !item.IsDone;
            });

            RaiseChanged();
        }

        private async void OnItemMore(Object obj)
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

        private void RaiseChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }

        public void HACK_ResetCityToTriggerRefresh()
        {
            var x = City;
            City = null;
            City = x;
        }
    }
}
