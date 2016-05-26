using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using System.Drawing;
using Mapdo;
using Mapdo.iOS;
using UIKit;
using CoreGraphics;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(ShapeView), typeof(ShapeRenderer))]
namespace Mapdo.iOS
{
    /// <summary>
    /// https://github.com/chrispellett/Xamarin-Forms-Shape/blob/master/LICENSE.txt
    /// 
    /// The MIT License (MIT)
    /// Copyright(c) 2015 Chris
    /// 
    /// Permission is hereby granted, free of charge, to any person obtaining a copy of
    /// this software and associated documentation files (the "Software"), to deal in
    /// the Software without restriction, including without limitation the rights to
    /// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
    /// the Software, and to permit persons to whom the Software is furnished to do so,
    /// subject to the following conditions:
    /// 
    /// The above copyright notice and this permission notice shall be included in all
    /// copies or substantial portions of the Software.
    /// 
    /// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    /// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
    /// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
    /// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
    /// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
    /// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    /// </summary>
    public class ShapeRenderer : VisualElementRenderer<ShapeView>
    {
        private readonly float QuarterTurnCounterClockwise = (float)(-1f * (Math.PI * 0.5f));

        public ShapeRenderer()
        {

        }

        public override void Draw(CGRect rect)
        {
            var currentContext = UIGraphics.GetCurrentContext();
            var properRect = AdjustForThickness(rect);
            HandleShapeDraw(currentContext, properRect);
        }

        protected RectangleF AdjustForThickness(CGRect rect)
        {
            var x = rect.X + Element.Padding.Left;
            var y = rect.Y + Element.Padding.Top;
            var width = rect.Width - Element.Padding.HorizontalThickness;
            var height = rect.Height - Element.Padding.VerticalThickness;
            return new RectangleF((float)x, (float)y, (float)width, (float)height);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == ShapeView.ColorProperty.PropertyName)
                this.SetNeedsDisplay(); // Redraw.
        }

        protected virtual void HandleShapeDraw(CGContext currentContext, RectangleF rect)
        {
            // Only used for circles
            var centerX = rect.X + (rect.Width / 2);
            var centerY = rect.Y + (rect.Height / 2);
            var radius = rect.Width / 2;
            var startAngle = 0;
            var endAngle = (float)(Math.PI * 2);

            switch (Element.ShapeType)
            {
                case ShapeType.Box:
                    HandleStandardDraw(currentContext, rect, () => {
                        if (Element.CornerRadius > 0)
                        {
                            var path = UIBezierPath.FromRoundedRect(rect, Element.CornerRadius);
                            currentContext.AddPath(path.CGPath);
                        }
                        else
                        {
                            currentContext.AddRect(rect);
                        }
                    });
                    break;
                case ShapeType.Circle:
                    HandleStandardDraw(currentContext, rect, () => currentContext.AddArc(centerX, centerY, radius, startAngle, endAngle, true));
                    break;
                case ShapeType.CircleIndicator:
                    HandleStandardDraw(currentContext, rect, () => currentContext.AddArc(centerX, centerY, radius, startAngle, endAngle, true));
                    HandleStandardDraw(currentContext, rect, () => currentContext.AddArc(centerX, centerY, radius, QuarterTurnCounterClockwise, (float)(Math.PI * 2 * (Element.IndicatorPercentage / 100)) + QuarterTurnCounterClockwise, false), Element.StrokeWidth + 3);
                    break;
            }
        }

        /// <summary>
        /// A simple method for handling our drawing of the shape. This method is called differently for each type of shape
        /// </summary>
        /// <param name="currentContext">Current context.</param>
        /// <param name="rect">Rect.</param>
        /// <param name="createPathForShape">Create path for shape.</param>
        /// <param name="lineWidth">Line width.</param>
        protected virtual void HandleStandardDraw(CGContext currentContext, RectangleF rect, Action createPathForShape, float? lineWidth = null)
        {
            currentContext.SetLineWidth(lineWidth ?? Element.StrokeWidth);
            currentContext.SetFillColor(base.Element.Color.ToCGColor());
            currentContext.SetStrokeColor(Element.StrokeColor.ToCGColor());

            createPathForShape();

            currentContext.DrawPath(CoreGraphics.CGPathDrawingMode.FillStroke);
        }
    }
}