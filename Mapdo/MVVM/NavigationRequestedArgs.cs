using System;

namespace Mapdo
{
    public class NavigationRequestedArgs : EventArgs
    {
        public IViewModel ViewModel { get; private set; }

        public NavigationRequestedArgs(IViewModel viewModel)
        {
            ViewModel = viewModel;
        }
    }
}