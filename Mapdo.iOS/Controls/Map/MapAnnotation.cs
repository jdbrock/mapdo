using System;
using MapKit;
using CoreLocation;
using Foundation;
using UIKit;
using Xamarin.Forms.Maps;

namespace Mapdo.iOS
{
	public class MapAnnotation<TModel> : MKAnnotation, IMapAnnotation where TModel : ExtendedPin	
	{
        // ===========================================================================
        // = Public Properties
        // ===========================================================================
        
        public TModel Model { get; private set; }

        public Position Location
        {
            get { return new Position(Coordinate.Latitude, Coordinate.Longitude); }
            set
            {
                if (value.Equals(Location))
                    return;

                UIView.Animate(1.0, () =>
                {
                    WillChangeValue("coordinate");
                    _coordinate = new CLLocationCoordinate2D(value.Latitude, value.Longitude);
                    DidChangeValue("coordinate");
                });
            }
        }

        public override CLLocationCoordinate2D Coordinate { get { return this._coordinate; } }

        // ===========================================================================
        // = Private Fields
        // ===========================================================================
        
        public CLLocationCoordinate2D _coordinate;

        // ===========================================================================
        // = Public Events
        // ===========================================================================
        
        public event EventHandler LocationChanged;

        // ===========================================================================
        // = Construction
        // ===========================================================================
        
        public MapAnnotation(TModel model)
		{
			Model = model;
			Location = model.Location;
		}

        // ===========================================================================
        // = Public Methods
        // ===========================================================================
        
        public override void SetCoordinate(CLLocationCoordinate2D coord)
		{
			_coordinate = coord;
			var handler = LocationChanged;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}
	}
}
