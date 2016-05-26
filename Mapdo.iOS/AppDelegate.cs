using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Themes;
using Xamarin.Themes.Core;
using Foundation;
using UIKit;
using Yelp;
using Yelp.iOS;
using Brock.Services;
using Yelp.Portable;
using System.Reflection;
using Akavache;

namespace Mapdo.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Services.GetBaseTypeFunc = T => T.BaseType;
            Services.GetInterfacesFunc = T => T.FindInterfaces(new TypeFilter((X, O) => true), null);
            Services.Register(new AssemblyService());

            global::Xamarin.Forms.Forms.Init();
            global::Xamarin.FormsMaps.Init();

            LoadApplication(new App());

            Services.Register(new YelpHttpClient("http://api.yelp.com/v2/",
                new Options
                {
                    AccessToken = App.Config.Yelp.AccessToken,
                    AccessTokenSecret = App.Config.Yelp.AccessTokenSecret,
                    ConsumerKey = App.Config.Yelp.ConsumerKey,
                    ConsumerSecret = App.Config.Yelp.ConsumerSecret
                }
            ));

            return base.FinishedLaunching(app, options);
        }
    }
}
