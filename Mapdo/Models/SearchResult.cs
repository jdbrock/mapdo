using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using YelpSharp;

namespace Mapdo.Models
{
    public class SearchResult
    {
        [Indexed]
        public String Name { get; set; }

        public Double Latitude { get; set; }
        public Double Longitude { get; set; }

        public String Address { get; set; }

        [Ignored]
        public Color Color => Color.FromRgb(193, 82, 216);

        public YelpBusiness YelpData { get; set; }
    }
}
