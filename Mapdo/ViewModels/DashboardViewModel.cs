﻿
using Acr.UserDialogs;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mapdo
{
    [ImplementPropertyChanged]
    public class DashboardViewModel : IViewModel
    {
        // ===========================================================================
        // = Public Properties
        // ===========================================================================
        
        public Trip Trip { get; set; }

        public ObservableCollection<Poi> SearchResults { get; set; }
        public ObservableCollection<ExtendedPin> Pins { get; set; }

        public Boolean IsSearching { get; set; }
        public ICommand OnItemDoneCommand { get; set; }
        public ICommand OnItemMoreCommand { get; set; }

        public event EventHandler Changed;
        public event EventHandler<NavigationRequestedArgs> NavigationRequested;

        public Boolean IsPortrait { get; set; }
        public Boolean IsLandscape {  get { return !IsPortrait; } }

        public DashboardView View { get; set; }

        // ===========================================================================
        // = Construction
        // ===========================================================================

        public DashboardViewModel(Trip trip)
        {
            Trip = trip;

            SearchResults = new ObservableCollection<Poi>();
            Pins = new ObservableCollection<ExtendedPin>();

            OnItemDoneCommand = new Command(OnItemDone);
            OnItemMoreCommand = new Command(OnItemMore);
        }

        // ===========================================================================
        // = Commands
        // ===========================================================================

        private void OnItemDone(Object obj)
        {
            var item = obj as Poi;
            item.IsDone = !item.IsDone;
        }

        private async void OnItemMore(Object obj)
        {
            var item = obj as Poi;
            var result = await View.DisplayActionSheet(item.Name, "Cancel", "Delete Place", "Show Information", "View in Yelp", "Book an Uber");
            var encode = (Func<String, String>)WebUtility.UrlEncode;

            switch (result)
            {
                case "Cancel":
                    break;

                case "Delete Place":
                    {
                        Trip.POIs.Remove(item);
                        RaiseChanged();
                        break;
                    }

                case "Show Information":
                    {
                        if (item.ExternalYelpData == null || String.IsNullOrWhiteSpace(item.ExternalYelpData.id))
                            UserDialogs.Instance.ShowError("Yelp data missing.");
                        else
                            Navigate(new PoiInformationViewModel(item));

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
                    if (item.ExternalYelpData == null || String.IsNullOrWhiteSpace(item.ExternalYelpData.id))
                    {
                        UserDialogs.Instance.ShowError("Yelp data missing.");
                    }
                    else
                    {
                        var uri = new Uri($"yelp:///biz/{item.ExternalYelpData.id}");
                        Device.OpenUri(uri);
                    }
                    break;
            }
        }

        private void Navigate(IViewModel viewModel)
        {
            var handler = NavigationRequested;
            if (handler != null)
                handler(this, new NavigationRequestedArgs(viewModel));
        }

        private void RaiseChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }
}
