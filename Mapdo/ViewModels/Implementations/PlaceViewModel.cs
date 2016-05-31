using Mapdo.Models;
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
    public class PlaceViewModel : ViewModelBase
    {
        // ===========================================================================
        // = Public Properties
        // ===========================================================================
        
        public Place Place { get; set; }
    }
}
