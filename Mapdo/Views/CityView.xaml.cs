using Acr.UserDialogs;
using Mapdo.Models;
using Mapdo.ViewModels;
using Realms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using YelpSharp;

namespace Mapdo.Views
{
    public class CityViewBase : ViewPage<CityViewModel> { }
    public partial class CityView : CityViewBase
    {
        // ===========================================================================
        // = Private Fields
        // ===========================================================================

        private double? _lastWidthAllocated;
        private double? _lastHeightAllocated;

        // ===========================================================================
        // = Construction
        // ===========================================================================

        public CityView()
        {
            InitializeComponent();
        }

        // ===========================================================================
        // = Change Notification
        // ===========================================================================

        public override void OnViewModelReplaced(IViewModel oldViewModel, IViewModel newViewModel)
        {
            UnhookChangeEvents(oldViewModel as CityViewModel);
            HookChangeEvents(newViewModel as CityViewModel);
        }

        private void HookChangeEvents(CityViewModel viewModel)
        {
            if (viewModel == null)
                return;

            viewModel.Changed += OnViewModelChanged;
            viewModel.RequestMapNavigation += OnMapNavigationRequested;
            viewModel.RequestMapNavigationExtent += OnMapNavigationExtentRequested;
        }

        private void UnhookChangeEvents(CityViewModel viewModel)
        {
            if (viewModel == null)
                return;

            viewModel.Changed -= OnViewModelChanged;
            viewModel.RequestMapNavigation -= OnMapNavigationRequested;
            viewModel.RequestMapNavigationExtent -= OnMapNavigationExtentRequested;
        }

        private void OnViewModelChanged(object sender, EventArgs e)
        {
            RecreatePins();
        }

        public override void OnViewModelRefreshed(object sender, EventArgs args)
        {
            Refresh();
        }

        private void Refresh()
        {
            RefreshRotation();
            RefreshMapExtent();
        }

        // ===========================================================================
        // = Rotation
        // ===========================================================================

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            _lastHeightAllocated = height;
            _lastWidthAllocated = width;

            RefreshRotation();
        }

        private void RefreshRotation()
        {
            if (!_lastHeightAllocated.HasValue || !_lastWidthAllocated.HasValue || ViewModel == null)
                return;

            var width = _lastWidthAllocated.Value;
            var height = _lastHeightAllocated.Value;

            if (height > width) // Portrait
            {
                Grid.SetColumn(itemsGrid, 0);
                Grid.SetRow(itemsGrid, 1);

                itemsGrid.WidthRequest = 0;
                itemsGrid.HeightRequest = height / 2;
            }
            else // Landscape
            {
                Grid.SetColumn(itemsGrid, 1);
                Grid.SetRow(itemsGrid, 0);

                itemsGrid.WidthRequest = width / 3;
                itemsGrid.HeightRequest = 0;
            }
        }

        // ===========================================================================
        // = Map
        // ===========================================================================
        
        protected override void OnAppearing()
        {
            RefreshMapExtent();
        }

        private void OnMapNavigationExtentRequested(object sender, Position[] e)
        {
            map.ZoomToExtent(e);
        }

        private void OnMapNavigationRequested(object sender, Place place)
        {
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(place.Latitude, place.Longitude), Distance.FromMiles(0.1)));
        }

        private void RefreshMapExtent()
        {
            if (ViewModel == null)
                return;

            var cityCenter = new Position(ViewModel.City.Latitude, ViewModel.City.Longitude);
            var citySpan = MapSpan.FromCenterAndRadius(cityCenter, Distance.FromMiles(10));

            map.MoveToRegion(citySpan);
            map.ItemsSource = ViewModel.Pins;

            RecreatePins();
        }

        private void RecreatePins()
        {
            ViewModel.Pins.Clear();

            if (ViewModel.City.Places != null)
                foreach (var place in ViewModel.City.Places)
                    ViewModel.Pins.Add(CreateSavedPinFromPlace(place));

            if (ViewModel.IsSearching)
                foreach (var result in ViewModel.SearchResults)
                    ViewModel.Pins.Add(CreateSearchResultPinFromPlace(result));

            map.RefreshPins();
        }

        private ExtendedPin CreateSavedPinFromPlace(Place place)
        {
            return new ExtendedPin(place.Name, place.Address, place.Latitude, place.Longitude)
            {
                PinColor = place.IsDone
                    ? StandardPinColor.Green
                    : StandardPinColor.Red,

                IsSearchResult = false
            };
        }

        private ExtendedPin CreateSearchResultPinFromPlace(SearchResult result)
        {
            return new ExtendedPin(result.Name, result.Address, result.Latitude, result.Longitude)
            {
                PinColor = StandardPinColor.Purple,
                IsSearchResult = true
            };
        }
    }
}
