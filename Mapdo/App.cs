using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Reflection;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

using PropertyChanged;
using Mapdo.ViewModels;
using Mapdo.Models;

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

        //public static Data Data { get; set; }
        public static Configuration Config { get; set; }

        // ===========================================================================
        // = Private Fields
        // ===========================================================================
        
        private static CitiesViewModel _mainViewModel;

        // ===========================================================================
        // = Construction
        // ===========================================================================
        
        public App(AppSetup setup)
        {
            AppContainer.Container = setup.CreateContainer(); 

            LoadConfiguration();
            LoadData();

            _mainViewModel = new CitiesViewModel();
            MainPage = new NavigationPage(new Views.CitiesView(_mainViewModel));
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
            //BlobCache.UserAccount.InsertObject("Configuration", Data);
        }

        // ===========================================================================
        // = Event Handling
        // ===========================================================================

        protected override void OnStart() { }
        protected override void OnResume() { }
        protected override void OnSleep()
        {
            //BlobCache.UserAccount.Flush();
        }

        // ===========================================================================
        // = Private Methods
        // ===========================================================================

        private static void ResetAllData()
        {
            //BlobCache.UserAccount.InvalidateAll();
        }

        private static void LoadData()
        {
            //BlobCache.UserAccount.GetObject<Data>("Configuration")
            //    .Subscribe(
            //    next =>
            //    {
            //        Data = next;
            //        _mainViewModel.Cities = Data.Trips;
            //    },
            //    error =>
            //    {
            //        CreateMockupData();
            //        _mainViewModel.Cities = Data.Trips;
            //    });
        }

        private static void CreateMockupData()
        {
            var city = new City
            {
                Name = "Portland, OR"
            };

            var gc = new Geocoder();
            var pos = gc.GetPositionsForAddressAsync(city.Name).Result.FirstOrDefault();

            if (pos != null)
            {
                city.Latitude = pos.Latitude;
                city.Longitude = pos.Longitude;
            }

            //Data.Trips.Add(trip);
            Save();
        }
    }
}
