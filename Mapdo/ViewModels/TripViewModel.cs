using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mapdo
{
    [ImplementPropertyChanged]
    public class TripViewModel : IViewModel
    {
        // ===========================================================================
        // = Public Properties
        // ===========================================================================
        
        public ObservableCollection<Trip> Trips { get; set; }

        // ===========================================================================
        // = Construction
        // ===========================================================================
        
        public TripViewModel()
        {
            Trips = new ObservableCollection<Trip>();
        }
    }
}
