using Mapdo.Integrations;
using Mapdo.Models;
using Mapdo.Services;
using PropertyChanged;
using Realms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mapdo.ViewModels
{
    [ImplementPropertyChanged]
    public class AddCityViewModel : ViewModelBase
    {
        public string SearchText { get; set; }
        public IList<TeleportCitySuggestion> Suggestions { get; private set; }

        public ICommand AddCityCommand { get; }

        private CancellationTokenSource _searchCancellationTokenSource;

        private INavigationService _navigationService;

        public AddCityViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            AddCityCommand = new Command(AddCity);
        }

        private async void AddCity(object obj)
        {
            if (obj as TeleportCitySuggestion == null)
                return;

            var teleportSuggestion = (TeleportCitySuggestion)obj;
            var teleportClient = new TeleportClient();
            var teleportData = await teleportClient.GetCity(teleportSuggestion.TeleportUri, CancellationToken.None); // HACK

            City city = null;

            var realm = Realm.GetInstance();
            realm.Write(() =>
            {
                city = realm.CreateObject<City>();
                city.Name = teleportData.Name;
                city.Latitude = teleportData.Latitude;
                city.Longitude = teleportData.Longitude;
            });

            await _navigationService.PopToRootAsync();
            await _navigationService.PushAsync<CityViewModel>(c => c.City = city);

            Clear();
        }

        private void Clear()
        {
            SearchText = String.Empty;
            Suggestions = null;
        }

        private void OnSearchTextChanged()
        {
            if (_searchCancellationTokenSource != null)
                _searchCancellationTokenSource.Cancel();

            _searchCancellationTokenSource = new CancellationTokenSource();

            var searchText = SearchText;
            var cts = _searchCancellationTokenSource;

            Task.Factory.StartNew(async () =>
            {
                try
                {
                    await Task.Delay(500, cts.Token);
                    var results = await new TeleportClient().SearchAsync(searchText, cts.Token);

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Suggestions = results;
                    });
                }
                catch { }
            }, cts.Token);
        }
    }
}