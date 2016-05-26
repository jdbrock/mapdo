using Xamarin.Forms.Maps;
using System.Collections.Generic;
using System.Linq;
using System;
using Mapdo;

namespace System
{
	public static class MapExtensions
	{
		public static IList<Pin> ToPins<T>(this IEnumerable<T> items) where T : ExtendedPin
		{
			return items.Select(i => i.AsPin()).ToList();
		}

		public static Pin AsPin(this ExtendedPin item)
		{
			return new Pin { Label = item.Name, Address = item.Details, Position = item.Location };
		}

        public static Boolean HasCustomPinImage(this ExtendedPin item)
        {
            return !String.IsNullOrWhiteSpace(item.CustomPinImageName);
        }

        public static Boolean HasPlaceImage(this ExtendedPin item)
        {
            return !String.IsNullOrWhiteSpace(item.ImageUrl);
        }
    }
}