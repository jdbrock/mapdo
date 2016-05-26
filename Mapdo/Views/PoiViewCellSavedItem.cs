using System;
using Xamarin.Forms;

namespace Mapdo
{
	public class PoiViewCellSavedItem : PoiViewCellBase
	{
		public PoiViewCellSavedItem()
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
            var viewModel = (DashboardViewModel)page.BindingContext;
            var poi = (Poi)BindingContext;

            viewModel.OnItemDoneCommand.Execute(poi);
        }

        private void OnMenuItemMoreClicked(object sender, EventArgs e)
        {
            var page = this.FindParent<ContentPage>();
            var viewModel = (DashboardViewModel)page.BindingContext;
            var poi = (Poi)BindingContext;

            viewModel.OnItemMoreCommand.Execute(poi);
        }
    }
}

