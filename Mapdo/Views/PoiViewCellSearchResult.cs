using System;
using Xamarin.Forms;

namespace Mapdo
{
    public class PoiViewCellSearchResult : PoiViewCellBase
    {
        protected override ShapeView CreateCircle()
        {
            var circle = new ShapeView()
            {
                ShapeType = ShapeType.Circle,
                WidthRequest = 16,
                HeightRequest = 16
            };

            circle.SetValue(ShapeView.ColorProperty, Color.FromRgb(193, 82, 216));

            return circle;
        }
    }
}

