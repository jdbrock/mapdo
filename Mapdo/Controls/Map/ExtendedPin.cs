using System;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using Mapdo;
using PropertyChanged;

namespace Mapdo
{
    [ImplementPropertyChanged]
	public class ExtendedPin
	{
        // ===========================================================================
        // = Public Properties
        // ===========================================================================
        
        public String Name { get; set; }
        public String Details { get; set; }
        public String ImageUrl { get; set; }
        public Position Location { get; set; }

        public String CustomPinImageName { get; set; }
        
        public Boolean IsSearchResult { get; set; }

        public StandardPinColor PinColor { get; set; }

        // ===========================================================================
        // = Construction
        // ===========================================================================

        public ExtendedPin(string name, string details, double latitude, double longitude)
		{
			Name     = name;
			Details  = details;
            Location = new Position(latitude, longitude);
		}
    }
}

