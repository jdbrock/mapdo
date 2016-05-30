﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Reflection;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

//using PropertyChanged;
using Mapdo.ViewModels;
using Mapdo.Models;
using Mapdo.Views;
using Realms;
using Mapdo.Bootstrap;

namespace Mapdo
{
    //[ImplementPropertyChanged]
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
        
        //private static CitiesViewModel _mainViewModel;

        // ===========================================================================
        // = Construction
        // ===========================================================================
        
        public App()
        {
            var bootstrapper = new AppBootstrapper(this);
            bootstrapper.Run();

            LoadConfiguration();
            CreateInitialData();
            LoadData();
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

        private static void CreateInitialData()
        {
            var realm = Realm.GetInstance();

            if (realm.All<City>().Any())
                return;

            realm.Write(() =>
            {
                var city = realm.CreateObject<City>();

                city.Name = "Portland, OR";
                city.Latitude = 45.5231d;
                city.Longitude = -122.6765d;
            });
        }
    }
}
