using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mapdo
{
    public abstract class PlaceViewCellBase : ViewCell
    {
        public PlaceViewCellBase()
        {
			View = CreateCell();
        }

		protected virtual View CreateCell()
		{
			var nameLabel = new Label()
			{
				FontFamily = "HelveticaNeue-Medium",
				FontSize = 16,
				TextColor = Color.Black,
				LineBreakMode = LineBreakMode.TailTruncation
			};
			nameLabel.SetBinding(Label.TextProperty, "Name");

			var addressLabel = new Label()
			{
				FontAttributes = FontAttributes.None,
				FontSize = 12,
				TextColor = Color.FromHex("#666"),
				LineBreakMode = LineBreakMode.TailTruncation
			};
			addressLabel.SetBinding(
				Label.TextProperty, new Binding("Address")
			);

			var textLayout = new StackLayout
			{
				Padding = new Thickness(10, 0, 0, 0),
				Spacing = 0,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = { nameLabel, addressLabel }
			};

            var circle = CreateCircle();

			var cellLayout = new StackLayout
			{
				Spacing = 0,
				Padding = new Thickness(10, 5, 10, 5),
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = { circle, textLayout }
			};

			return cellLayout;
		}

        protected abstract ShapeView CreateCircle();
    }
}
