﻿using System;
using System.Collections.Generic;
using System.Linq;
//using Xamarin.Themes;
//using Xamarin.Themes.Core;
using Foundation;
using UIKit;
using System.Reflection;
using HockeyApp;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

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
        public override bool FinishedLaunching(UIApplication uiApp, NSDictionary options)
        {
            // Initialize Xamarin (needs to happen before App instantiation).
            InitializeXamarin();

            // Create app.
            var app = new App();

            // Further initialization.
            InitializeHockeyApp();
            //InitializeLegacyServices();

            // Load app.
            LoadApplication(app);

            ConfigureTheming();

            return base.FinishedLaunching(uiApp, options);
        }

        private void ConfigureTheming()
        {
            var tint = Color.FromHex("8e44ad").ToUIColor();

            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);

            UINavigationBar.Appearance.TintColor = UIColor.White;
            UINavigationBar.Appearance.BarTintColor = tint;
            UIBarButtonItem.Appearance.SetTitleTextAttributes(new UITextAttributes { TextColor = UIColor.White }, UIControlState.Normal);

            UISearchBar.Appearance.TintColor = tint;
            UIBarButtonItem.Appearance.TintColor = tint;

            UIView.Appearance.TintColor = tint;
        }

        private void InitializeHockeyApp()
        {
            var hockeyManager = BITHockeyManager.SharedHockeyManager;

            hockeyManager.Configure(App.Config.HockeyAppId);
            hockeyManager.DebugLogEnabled = true;
            hockeyManager.StartManager();
        }

        //private void InitializeLegacyServices()
        //{
        //    Services.Initialize(
        //        T => T.BaseType,
        //        T => T.FindInterfaces((X, Y) => true, null));

        //    Services.Register(new AssemblyService());
        //}

        private void InitializeXamarin()
        {
            global::Xamarin.Forms.Forms.Init();
            global::Xamarin.FormsMaps.Init();
        }
    }
}
