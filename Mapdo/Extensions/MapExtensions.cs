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

            //if (positions.Count > 1)
            //{
            //    foreach (var item in positions.Skip(1))
            //    {
            //        if (item.Latitude > latitudeMax)
            //            latitudeMax = item.Latitude;

            //        if (item.Latitude < latitudeMin)
            //            latitudeMin = item.Latitude;

            //        if (item.Longitude > longitudeMax)
            //            longitudeMax = item.Longitude;

            //        if (item.Longitude < longitudeMin)
            //            longitudeMin = item.Longitude;
            //    }

            //    latitudeSpan    = latitudeMax  - latitudeMin;
            //    longitudeSpan   = longitudeMax - longitudeMin;
            //    latitudeCenter  = latitudeMin  + (latitudeSpan / 2);
            //    longitudeCenter = longitudeMin + (longitudeSpan / 2);
            //}

            var position = new Position(latitudeCenter, longitudeCenter);
            @this.MoveToRegion(new MapSpan(position, latitudeSpan, longitudeSpan));
        }
    }
}
