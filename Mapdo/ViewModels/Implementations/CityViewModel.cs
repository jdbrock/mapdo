
//using Acr.UserDialogs;
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

namespace Mapdo.ViewModels
{
    [ImplementPropertyChanged]
    public class CityViewModel : ViewModelBase
    {
        // ===========================================================================
        // = Public Properties
        // ===========================================================================
        
        public City City { get; set; }

        //public IEnumerable<Place> Places { get; set; }
        public ObservableCollection<SearchResult> SearchResults { get; set; }
        public ObservableCollection<ExtendedPin> Pins { get; set; }

        public Boolean IsSearching { get; set; }
        public ICommand OnItemDoneCommand { get; set; }
        public ICommand OnItemMoreCommand { get; set; }

        //public event EventHandler Changed;
        //public event EventHandler<NavigationRequestedArgs> NavigationRequested;

        public Views.CityView View { get; set; }

        // ===========================================================================
        // = Construction
        // ===========================================================================

        public CityViewModel()
        {
            SearchResults = new ObservableCollection<SearchResult>();
            Pins = new ObservableCollection<ExtendedPin>();

            OnItemDoneCommand = new Command(OnItemDone);
            OnItemMoreCommand = new Command(OnItemMore);

            //Realm.GetInstance().RealmChanged += (S, E) =>
            //{
            //    // Hack to fresh from Realm changes.
            //    var pins = Pins;
            //    Pins = null;
            //    Pins = pins;

            //    var city = City;
            //    City = null;
            //    City = city;
            //};
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
        }

        private async void OnItemMore(Object obj)
        {
            var item = obj as Place;
            var result = await View.DisplayActionSheet(item.Name, "Cancel", "Delete Place", "Show Info", "View in Yelp", "Book an Uber");
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
                        break;
                    }

                // TODO: Reinstate Show Info
                //case "Show Info":
                //    {
                //        if (item.ExternalYelpData == null || String.IsNullOrWhiteSpace(item.ExternalYelpData.id))
                //            UserDialogs.Instance.ShowError("Yelp data missing.");
                //        else
                //            Navigate(new PlaceInformationViewModel(item));

                //        break;
                //    }

                case "Book an Uber":
                    {
                        var uri = new Uri($"uber://?action=setPickup&pickup=my_location&client_id=Mapdo"
                            + $"&dropoff[latitude]={item.Latitude}&dropoff[longitude]={item.Longitude}"
                            + $"&dropoff[nickname]={encode(item.Name)}&dropoff[formatted_address]={encode(item.Address)}");
                        Device.OpenUri(uri);
                        break;
                    }

                // TODO: Reinstate View in Yelp
                //case "View in Yelp":
                //    if (item.ExternalYelpData == null || String.IsNullOrWhiteSpace(item.ExternalYelpData.id))
                //        UserDialogs.Instance.ShowError("Yelp data missing.");
                //    else
                //    {
                //        var uri = new Uri($"yelp:///biz/{item.ExternalYelpData.id}");
                //        Device.OpenUri(uri);
                //    }
                //    break;
            }
        }

        //private void Navigate(IViewModel viewModel)
        //{
        //    var handler = NavigationRequested;
        //    if (handler != null)
        //        handler(this, new NavigationRequestedArgs(viewModel));
        //}

        //private void RaiseChanged()
        //{
        //    Changed?.Invoke(this, EventArgs.Empty);
        //}
    }
}
