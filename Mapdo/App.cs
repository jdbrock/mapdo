using Akavache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive;
using System.Reactive.Linq;

using Xamarin.Forms;
using PropertyChanged;
using Xamarin.Forms.Maps;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Reflection;

namespace Mapdo
{
    [ImplementPropertyChanged]
    public class App : Application
    {
        // ===========================================================================
        // = Constants
        // ===========================================================================

        private const string _configResourcePath = "Mapdo.config.secrets";

        // ===========================================================================
        // = Public Properties
        // ===========================================================================

        public static Data Data { get; set; }
        public static Configuration Config { get; set; }

        // ===========================================================================
        // = Private Fields
        // ===========================================================================
        
        private static TripViewModel _mainViewModel;

        // ===========================================================================
        // = Construction
        // ===========================================================================
        
        public App()
        {
            BlobCache.ApplicationName = "Mapdo";
            BlobCache.EnsureInitialized();

            //ResetAllData();

            Data = new Data();

            LoadConfiguration();
            LoadData();

            _mainViewModel = new TripViewModel();
            MainPage = new NavigationPage(new TripView(_mainViewModel));
        }

        private void LoadConfiguration()
        {
            var configAssembly = typeof(Configuration).GetTypeInfo().Assembly;

            using (var configStream = configAssembly.GetManifestResourceStream(_configResourcePath))
                Config = (Configuration)new XmlSerializer(typeof(Configuration)).Deserialize(configStream);
        }

        // ===========================================================================
        // = Public Methods
        // ===========================================================================

        public static void Save()
        {
            BlobCache.UserAccount.InsertObject("Configuration", Data);
        }

        // ===========================================================================
        // = Event Handling
        // ===========================================================================

        protected override void OnStart() { }
        protected override void OnResume() { }
        protected override void OnSleep()
        {
            BlobCache.UserAccount.Flush();
        }

        // ===========================================================================
        // = Private Methods
        // ===========================================================================

        private static void ResetAllData()
        {
            BlobCache.UserAccount.InvalidateAll();
        }

        private static void LoadData()
        {
            BlobCache.UserAccount.GetObject<Data>("Configuration")
                .Subscribe(
                next =>
                {
                    Data = next;
                    _mainViewModel.Trips = Data.Trips;
                },
                error =>
                {
                    CreateMockupData();
                    _mainViewModel.Trips = Data.Trips;
                });
        }

        private static void CreateMockupData()
        {
            var trip = new Trip("Portland, OR");

            var gc = new Geocoder();
            var pos = gc.GetPositionsForAddressAsync(trip.Name).Result.FirstOrDefault();

            if (pos != null)
            {
                trip.Latitude = pos.Latitude;
                trip.Longitude = pos.Longitude;
            }

            Data.Trips.Add(trip);
            Save();
        }
    }
}
