using Mapdo.Models;
using PropertyChanged;
using Realms;
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
    public class CitiesViewModel : IViewModel
    {
        // ===========================================================================
        // = Public Properties
        // ===========================================================================
        
        public RealmList<City> Cities { get; set; }

        // ===========================================================================
        // = Construction
        // ===========================================================================
        
        public CitiesViewModel()
        {
            //Cities = new ObservableCollection<City>();
        }
    }
}
