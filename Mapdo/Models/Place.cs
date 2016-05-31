using PropertyChanged;
using Realms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using YelpSharp;

namespace Mapdo.Models
{
    public class Place : RealmObject
    {
        [Indexed]
        public String Name { get; set; }

        public Double Latitude { get; set; }
        public Double Longitude { get; set; }

        public String Address { get; set; }

        public Boolean IsDone { get; set; }

        // TODO: Reinstate ExternalYelpData
        //public YelpBusiness ExternalYelpData { get; set; }
    }
}
