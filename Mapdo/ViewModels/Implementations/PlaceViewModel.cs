using Mapdo.Models;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mapdo.ViewModels
{
    [ImplementPropertyChanged]
    public class PlaceViewModel : ViewModelBase
    {
        // ===========================================================================
        // = Public Properties
        // ===========================================================================
        
        public Place Place { get; set; }

        // ===========================================================================
        // = Construction
        // ===========================================================================
        
        public PlaceViewModel(Place place)
        {
            Place = place;
        }
    }
}
