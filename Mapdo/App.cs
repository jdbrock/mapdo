using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Mapdo.ViewModels;
using Mapdo.Models;
using Mapdo.Views;
using Realms;
using Mapdo.Bootstrap;

namespace Mapdo
{
    public class App : Application
    {
        // ===========================================================================
        // = Constants
        // ===========================================================================

        private const string _configResourcePath = "Mapdo.config.secrets";

        // ===========================================================================
        // = Public Properties
        // ===========================================================================

        public static Configuration Config { get; set; }

        // ===========================================================================
        // = Construction
        // ===========================================================================
        
        public App()
        {
            var bootstrapper = new AppBootstrapper(this);
            bootstrapper.Run();

            LoadConfiguration();
            //CreateInitialData();
        }

        private void LoadConfiguration()
        {
            var configAssembly = typeof(Configuration).GetTypeInfo().Assembly;

            using (var configStream = configAssembly.GetManifestResourceStream(_configResourcePath))
                Config = (Configuration)new XmlSerializer(typeof(Configuration)).Deserialize(configStream);
        }

        // ===========================================================================
        // = Private Methods
        // ===========================================================================

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
