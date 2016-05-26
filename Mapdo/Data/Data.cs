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
    public class Data
    {
        // ===========================================================================
        // = Public Properties
        // ===========================================================================
        
        public ObservableCollection<Trip> Trips { get; set; }

        // ===========================================================================
        // = Construction
        // ===========================================================================
        
        public Data()
        {
            Trips = new ObservableCollection<Trip>();
        }
    }
}
