using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapdo
{
    [ImplementPropertyChanged]
    public class Trip
    {
        // ===========================================================================
        // = Public Properties
        // ===========================================================================
        
        public String Name { get; set; }

        public Double Latitude { get; set; }
        public Double Longitude { get; set; }

        public ObservableCollection<Poi> POIs { get; set; }

        // ===========================================================================
        // = Construction
        // ===========================================================================
        
        public Trip() { }
        public Trip(String name)
        {
            Name = name;
            POIs = new ObservableCollection<Poi>();
        }
    }
}
