using Mapdo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Mapdo.Views
{
    public class PlaceViewBase : ViewPage<PlaceViewModel> { }
    public partial class PlaceView : PlaceViewBase
    {
        public PlaceView()
        {
            InitializeComponent();
        }
    }
}
