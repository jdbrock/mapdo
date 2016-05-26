using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Mapdo
{
    [ViewFor(typeof(PoiInformationViewModel))]
    public partial class PoiInformationView : ContentPage
    {
        public PoiInformationView(IViewModel viewModel)
        {
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}
