﻿using Mapdo.Models;
using Mapdo.ViewModels;
using System;
using Xamarin.Forms;

namespace Mapdo
{
	public class PlaceViewCellSavedItem : PlaceViewCellBase
	{
		public PlaceViewCellSavedItem()
		{
            var menuItemMore = new MenuItem();
            menuItemMore.Text = "Options";
            menuItemMore.IsDestructive = false;
            menuItemMore.Clicked += OnMenuItemMoreClicked;
            ContextActions.Add(menuItemMore);

            var menuItemDone = new MenuItem();
            menuItemDone.Text = "Done";
            menuItemDone.IsDestructive = false;
            menuItemDone.Clicked += OnMenuItemDoneClicked;
            ContextActions.Add(menuItemDone);
        }

        private void OnMenuItemDoneClicked(object sender, EventArgs e)
        {
            var page = this.FindParent<ContentPage>();
            var viewModel = (CityViewModel)page.BindingContext;
            var place = (Place)BindingContext;

            viewModel.SavedPlaceDoneCommand.Execute(place);
        }

        private void OnMenuItemMoreClicked(object sender, EventArgs e)
        {
            var page = this.FindParent<ContentPage>();
            var viewModel = (CityViewModel)page.BindingContext;
            var place = (Place)BindingContext;

            viewModel.SavedPlaceMoreCommand.Execute(place);
        }

        protected override ShapeView CreateCircle()
        {
            var circle = new ShapeView()
            {
                ShapeType = ShapeType.Circle,
                WidthRequest = 16,
                HeightRequest = 16
            };

            circle.SetBinding(ShapeView.ColorProperty, new Binding("IsDone", BindingMode.OneWay, new PlaceToColorConverter()));

            return circle;
        }
    }
}

