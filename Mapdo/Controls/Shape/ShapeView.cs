using System;
using Xamarin.Forms;

namespace Mapdo
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
    public class ShapeView : BoxView
    {
        public static readonly BindableProperty ShapeTypeProperty = BindableProperty.Create<ShapeView, ShapeType>(s => s.ShapeType, ShapeType.Box);

        public static readonly BindableProperty StrokeColorProperty = BindableProperty.Create<ShapeView, Color>(s => s.StrokeColor, Color.Default);

        public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create<ShapeView, float>(s => s.StrokeWidth, 1f);

        public static readonly BindableProperty IndicatorPercentageProperty = BindableProperty.Create<ShapeView, float>(s => s.IndicatorPercentage, 0f);

        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create<ShapeView, float>(s => s.CornerRadius, 0f);

        public static readonly BindableProperty PaddingProperty = BindableProperty.Create<ShapeView, Thickness>(s => s.Padding, default(Thickness));

        public ShapeType ShapeType
        {
            get { return (ShapeType)GetValue(ShapeTypeProperty); }
            set { SetValue(ShapeTypeProperty, value); }
        }

        public Color StrokeColor
        {
            get { return (Color)GetValue(StrokeColorProperty); }
            set { SetValue(StrokeColorProperty, value); }
        }

        public float StrokeWidth
        {
            get { return (float)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }

        public float IndicatorPercentage
        {
            get { return (float)GetValue(IndicatorPercentageProperty); }
            set
            {
                if (ShapeType != ShapeType.CircleIndicator)
                    throw new ArgumentException("Can only specify this property with CircleIndicator");
                SetValue(IndicatorPercentageProperty, value);
            }
        }

        public float CornerRadius
        {
            get { return (float)GetValue(CornerRadiusProperty); }
            set
            {
                if (ShapeType != ShapeType.Box)
                    throw new ArgumentException("Can only specify this property with Box");
                SetValue(CornerRadiusProperty, value);
            }
        }

        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        public ShapeView()
        {
        }
    }

    public enum ShapeType
    {
        Box,
        Circle,
        CircleIndicator
    }
}