using PropertyChanged;
using Realms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapdo.Models
{
    [ImplementPropertyChanged]
    public class City : RealmObject
    {
        [Indexed]
        public String Name { get; set; }

        public Double Latitude { get; set; }
        public Double Longitude { get; set; }

        public RealmList<Place> Places { get; }
    }
}
