using System;
using MapKit;
using CoreLocation;
using Foundation;
using UIKit;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using CoreGraphics;
using System.Threading.Tasks;
using System.Net.Http;
using Mapdo;

namespace Mapdo.iOS
{
    public class ExtendedMapDelegate : MKMapViewDelegate
    {
        private const String DEFAULT_IMAGE_REUSE_ID = "DEFAULT_IMG";
        private const String DEFAULT_PUSHPIN_REUSE_ID = "DEFAULT_PIN";

        private readonly Boolean _pinIsDraggable;
        private readonly Boolean _canShowCallout;
        private readonly ICommand _detailCommand;

        public ExtendedMapDelegate(bool pinIsDraggable = true, bool canShowCallout = false, ICommand detailCommand = null)
        {
            _pinIsDraggable = pinIsDraggable;
            _canShowCallout = canShowCallout;
            _detailCommand = detailCommand;
        }

        public override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            if (!(annotation is CustomAnnotation))
                return null;

            var model = ((CustomAnnotation)annotation).Model;
            var view = mapView.DequeueReusableAnnotation(GetReuseIdForPin(model));

            if (view == null)
                view = CreateViewForPin(annotation, model);
            else
                view.Annotation = annotation;

            view.Draggable = _pinIsDraggable;
            view.CanShowCallout = _canShowCallout;

            if (_canShowCallout)
            {
                if (model.HasPlaceImage())
                    view.LeftCalloutAccessoryView = GetImage(model);

                view.RightCalloutAccessoryView = GetDetailButton(model);
            }

            return view;
        }

        private MKAnnotationView CreateViewForPin(IMKAnnotation annotation, ExtendedPin model)
        {
            if (model.HasCustomPinImage())
            {
                var pinImageName = $"{model.CustomPinImageName}.png";

                var view = new MapAnnotationView(annotation, GetReuseIdForPin(model));
                view.Image = UIImage.FromBundle(pinImageName);
                view.CenterOffset = new CGPoint(0, -15);

                return view;
            }
            else
            {
                var view = new MKPinAnnotationView(annotation, GetReuseIdForPin(model));
                view.PinColor = GetPinColor(model.PinColor);

                return view;              
            }
        }

        private MKPinAnnotationColor GetPinColor(StandardPinColor pinColor)
        {
            switch (pinColor)
            {
                case StandardPinColor.Green:
                    return MKPinAnnotationColor.Green;

                case StandardPinColor.Red:
                    return MKPinAnnotationColor.Red;

                case StandardPinColor.Purple:
                    return MKPinAnnotationColor.Purple;
            }

            return MKPinAnnotationColor.Red;
        }

        private String GetReuseIdForPin(ExtendedPin model)
        {
            if (model.HasCustomPinImage())
                return $"{DEFAULT_IMAGE_REUSE_ID}{model.CustomPinImageName ?? ""}";

            return $"{DEFAULT_PUSHPIN_REUSE_ID}{model.PinColor}";
        }

        private UIButton GetDetailButton(ExtendedPin poi)
        {
            var detailButton = UIButton.FromType(UIButtonType.DetailDisclosure);
            detailButton.TouchUpInside += (s, e) =>
            {
                if (_detailCommand != null)
                    _detailCommand.Execute(poi);
            };

            return detailButton;
        }

        private UIImageView GetImage(ExtendedPin poi)
        {
            var imageView = new UIImageView(new CGRect(5d, 5d, 75d, 75d));

            if (poi.ImageUrl != null)
                Task.Run(async () =>
                {
                    var image = await LoadImage(poi.ImageUrl);

                    InvokeOnMainThread(() =>
                    {
                        imageView.Image = image;
                    });
                });

            return imageView;
        }

        private async Task<UIImage> LoadImage(string imageUrl)
        {
            var client = new HttpClient();
            var bytes  = await client.GetByteArrayAsync(imageUrl);

            return UIImage.LoadFromData(NSData.FromArray(bytes));
        }
    }
}
