using Xamarin.Forms.Maps;
using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Xamarin.Forms;

namespace Mapdo
{
	public class ExtendedMap : Map
	{
        // ===========================================================================
        // = Public Properties
        // ===========================================================================

        public ICommand ShowDetailCommand { get; set; }
        public MapSpan LastMoveToRegion { get; private set; }
        public IEnumerable ItemsSource { get; set; }

        public new MapSpan VisibleRegion
        {
            get { return _visibleRegion; }
            set
            {
                if (_visibleRegion == value)
                {
                    return;
                }
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                OnPropertyChanging("VisibleRegion");
                _visibleRegion = value;
                OnPropertyChanged("VisibleRegion");
            }
        }

        // ===========================================================================
        // = Bindable Properties
        // ===========================================================================

        public static readonly BindableProperty SelectedPinProperty = BindableProperty.Create(nameof(SelectedPin), typeof(ExtendedPin), typeof(ExtendedMap));
        public ExtendedPin SelectedPin
        {
            get { return (ExtendedPin)base.GetValue(SelectedPinProperty); }
            set { base.SetValue(SelectedPinProperty, value); }
        }

        // ===========================================================================
        // = Private Fields
        // ===========================================================================
        
        private MapSpan _visibleRegion;

        // ===========================================================================
        // = Public Events
        // ===========================================================================
        
        public event EventHandler PinsChanged;

        // ===========================================================================
        // = Construction
        // ===========================================================================
        
        public ExtendedMap() { }
		public ExtendedMap(MapSpan region) : base(region)
		{
			LastMoveToRegion = region;
		}

        // ===========================================================================
        // = Public Methods
        // ===========================================================================
        
        public void RefreshPins()
        {
            Pins.Clear();

            foreach (var item in ItemsSource.Cast<ExtendedPin>())
                Pins.Add(item.AsPin());

            RaisePinsChanged();
        }

        // ===========================================================================
        // = Private Methods
        // ===========================================================================
        
        private void RaisePinsChanged()
        {
            PinsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

