using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Mapdo;
using Xamarin.Forms.Platform.iOS;
using Mapdo.iOS;
using UIKit;
using CoreGraphics;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(RoundedButton), typeof(RoundedButtonRenderer))]
namespace Mapdo.iOS
{
    public class RoundedButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var button = (RoundedButton)e.NewElement;

                button.SizeChanged += (s, args) =>
                {
                    var radius = Math.Min(button.Width, button.Height) / 2.0;
                    button.BorderRadius = (int)(radius);
                };

                // TODO: Not yet working.
                var shadowPath = UIBezierPath.FromRoundedRect(Bounds, (int)Element.BorderRadius);
                Layer.ShadowPath = shadowPath.CGPath;
                Layer.ShadowColor = UIColor.Black.CGColor;
                Layer.ShadowOffset = new CGSize(width: 0.0, height: 0.5);
                Layer.ShadowOpacity = 0.2f;
                Layer.MasksToBounds = false;
            }
        }
    }
}