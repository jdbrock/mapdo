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
    public class PoiInformationViewModel : IViewModel
    {
        // ===========================================================================
        // = Public Properties
        // ===========================================================================
        
        public Poi Poi { get; set; }

        // ===========================================================================
        // = Construction
        // ===========================================================================
        
        public PoiInformationViewModel(Poi poi)
        {
            Poi = poi;
        }
    }
}
