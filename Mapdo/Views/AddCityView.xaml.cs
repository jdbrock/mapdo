using Acr.UserDialogs;
using Mapdo.Models;
using Mapdo.Services;
using Mapdo.ViewModels;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Mapdo.Views
{
    public class AddCityViewBase : ViewPage<AddCityViewModel> { }
    public partial class AddCityView : AddCityViewBase
    {
        // ===========================================================================
        // = Private Fields
        // ===========================================================================

        private INavigationService _navigation;

        // ===========================================================================
        // = Construction
        // ===========================================================================

        public AddCityView(INavigationService navigation)
        {
            _navigation = navigation;

            InitializeComponent();
        }
    }
}
