using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Mapdo
{
    public static class MapExtensions
    {
        public static void ZoomToExtent(this Map @this, IList<Position> positions)
        {
            if (positions.Count == 0)
                return;

            Double latitudeMin, latitudeMax, longitudeMin, longitudeMax, latitudeCenter, longitudeCenter, latitudeSpan, longitudeSpan;

            latitudeCenter  = latitudeMin  = latitudeMax  = positions[0].Latitude;
            longitudeCenter = longitudeMin = longitudeMax = positions[0].Longitude;

            latitudeSpan  = 0.0025;
            longitudeSpan = 0.0025;

            var position = new Position(latitudeCenter, longitudeCenter);
            @this.MoveToRegion(new MapSpan(position, latitudeSpan, longitudeSpan));
        }
    }
}
