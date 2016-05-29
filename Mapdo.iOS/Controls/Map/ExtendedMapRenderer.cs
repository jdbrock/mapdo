﻿using MapKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.Maps;
using System.Collections.Specialized;
using CoreLocation;
using CoreGraphics;
using System.ComponentModel;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using UIKit;
using System.Threading.Tasks;
using Xamarin.Geolocation;
//using BigTed;
using System.Linq;
using System;
using Mapdo;
using Mapdo.iOS;

[assembly: ExportRenderer(typeof(ExtendedMap), typeof(ExtendedMapRenderer))]
namespace Mapdo.iOS
{
	public class ExtendedMapRenderer : ViewRenderer<ExtendedMap, MKMapView>
	{
		//private UIButton _centerOnUserLocation; 

		protected override void Dispose(bool disposing)
		{
            //if (disposing)
            //{
            //	((ObservableCollection<Pin>)Element.Pins).CollectionChanged -= OnCollectionChanged;
            //}

            Element.PinsChanged -= OnPinsChanged;

			base.Dispose(disposing);
		}

        private void OnPinsChanged(object sender, EventArgs e)
        {
            UpdatePins();
        }

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			return new SizeRequest(new Xamarin.Forms.Size(40, 40));
		}

		private void MoveToRegion(MapSpan mapSpan)
		{
			Xamarin.Forms.Maps.Position center = mapSpan.Center;
			var region = new MKCoordinateRegion(new CLLocationCoordinate2D(center.Latitude, center.Longitude), new MKCoordinateSpan(mapSpan.LatitudeDegrees, mapSpan.LongitudeDegrees));
			Control.SetRegion(region, true);
		}

		//private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		//{
		//	UpdatePins();
		//}

		void MapMoveToRegion(Map map, MapSpan span)
		{
			MoveToRegion(span);
		}

		protected override void OnElementChanged(ElementChangedEventArgs<ExtendedMap> e)
		{
            base.OnElementChanged(e);

            if (e.NewElement == null)
                return;

			SetNativeControl(new MKMapView(CGRect.Empty));
			var mapModel = Element;
			MKMapView mkMapView = Control;
			mkMapView.RegionChanged += delegate 
			{
				if (Element == null)
				{
					return;
				}
				mapModel.VisibleRegion = new MapSpan(new Xamarin.Forms.Maps.Position(mkMapView.Region.Center.Latitude, mkMapView.Region.Center.Longitude), mkMapView.Region.Span.LatitudeDelta, mkMapView.Region.Span.LongitudeDelta);
			};

			MessagingCenter.Subscribe<Map, MapSpan>(this, "MapMoveToRegion", MapMoveToRegion, mapModel);
			if (mapModel.LastMoveToRegion != null)
            {
				MoveToRegion(mapModel.LastMoveToRegion);
			}

			SetupMapView();

			UpdateMapType();
			UpdateIsShowingUser();
			UpdateHasScrollEnabled();
			UpdateHasZoomEnabled();

            Element.PinsChanged += OnPinsChanged;

			//((ObservableCollection<Pin>)Element.Pins).CollectionChanged += OnCollectionChanged;
			UpdatePins();
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == Map.MapTypeProperty.PropertyName)
			{
				UpdateMapType();
				return;
			}
			if (e.PropertyName == Map.IsShowingUserProperty.PropertyName)
			{
				UpdateIsShowingUser();
				return;
			}
			if (e.PropertyName == Map.HasScrollEnabledProperty.PropertyName)
			{
				UpdateHasScrollEnabled();
				return;
			}
			if (e.PropertyName == Map.HasZoomEnabledProperty.PropertyName)
			{
				UpdateHasZoomEnabled();
			}
		}

		private void UpdateHasScrollEnabled()
		{
			Control.ScrollEnabled = Element.HasScrollEnabled;
		}

		private void UpdateHasZoomEnabled()
		{
			Control.ZoomEnabled = Element.HasZoomEnabled;
		}

		private void UpdateIsShowingUser()
		{
			Control.ShowsUserLocation = Element.IsShowingUser;
		}

		private void UpdateMapType()
		{
			switch (Element.MapType)
			{
				case MapType.Street:
					Control.MapType = MKMapType.Standard;
					return;
				case MapType.Satellite:
					Control.MapType = MKMapType.Satellite;
					return;
				case MapType.Hybrid:
					Control.MapType = MKMapType.Hybrid;
					return;
				default:
					return;
			}
		}

		private void UpdatePins()
		{
			Control.RemoveAnnotations(Control.Annotations);
            IList<ExtendedPin> pins = Element.ItemsSource?.Cast<ExtendedPin>().ToList();

            if (pins != null)
			    foreach (ExtendedPin current in pins)
				    Control.AddAnnotation(new CustomAnnotation(current));
		}

		private void SetupMapView ()
		{
			var map = Element;
			var mapView = Control;
			var detailCommand = map.ShowDetailCommand;

			//_centerOnUserLocation = new UIButton ();
			//_centerOnUserLocation.SetImage(UIImage.FromBundle("center.png"),UIControlState.Normal);
			//_centerOnUserLocation.AutoresizingMask = (UIViewAutoresizing.FlexibleTopMargin & UIViewAutoresizing.FlexibleLeftMargin);

			//PositionControls(UIApplication.SharedApplication.StatusBarOrientation);
			//mapView.Add (_centerOnUserLocation);

			mapView.Delegate = null;
			mapView.Delegate = new ExtendedMapDelegate(false, true, detailCommand); // No dragging, but with Callout. Provide command to call when the callout button is clicked
			//mapView.SetRegion (MKCoordinateRegion.FromDistance (new CLLocationCoordinate2D (0, 0), 5000, 5000), true);

			//_centerOnUserLocation.TouchUpInside += async (sender, e) => await SetMapCenterToUserLocation();
		}

		private async Task SetMapCenterToUserLocation()
		{
			var geoLocator = new Geolocator();
			try {
				//BTProgressHUD.Show(maskType: ProgressHUD.MaskType.Black); //shows the spinner
				var position = await geoLocator.GetPositionAsync (10000);
				//BTProgressHUD.Dismiss(); //dismiss the spinner

				SetMapCenter (position.Latitude, position.Longitude, UserLocationZoom);
			} 
			catch (GeolocationException) 
			{
				// Do something useful here
				SetMapCenter (0, 0, DefaultZoom);
			}
		}
			
		private const int DefaultZoom = 5000;
		private const int UserLocationZoom = 500;

		private void SetMapCenter(double latitude, double longitude, int distance)
		{
			var mapView = Control;
			var center = new CLLocationCoordinate2D(latitude, longitude);

			mapView.SetCenterCoordinate (center, true);
			mapView.SetRegion(MKCoordinateRegion.FromDistance(center,distance,distance), true);		
		}

	
		//void PositionControls (UIInterfaceOrientation toInterfaceOrientation)
		//{
		//	var bounds = UIScreen.MainScreen.Bounds;
		//	const int navBarOffset = 44; //UIViewExtensions.GetNavBarOffset(NavigationController);
		//	if (toInterfaceOrientation == UIInterfaceOrientation.Portrait ||
		//	    toInterfaceOrientation == UIInterfaceOrientation.PortraitUpsideDown) {
		//		_centerOnUserLocation.Frame = new CGRect (bounds.Width - 50, bounds.Height - navBarOffset - 50, 44, 44);
		//	} 
		//	else 
		//	{
		//		_centerOnUserLocation.Frame = new CGRect (bounds.Height - 50, bounds.Width - navBarOffset - 50, 44, 44);
		//	}
		//}

		private static float GetNavBarOffset(UINavigationController navController)
		{
			if (navController == null || navController.NavigationBar == null)
				return 0.0f;

			var navBarFrame = navController.NavigationBar.Frame;
			return (navController.NavigationBarHidden) 
				? (float)navBarFrame.Y 
					: (float)navBarFrame.Y + (float)navBarFrame.Height;
		}
	}
}