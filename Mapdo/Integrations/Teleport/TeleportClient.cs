using Mapdo.Models;
using RestSharp.Portable;
using RestSharp.Portable.Deserializers;
using RestSharp.Portable.HttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mapdo.Integrations
{
    public class TeleportClient
    {
        private static readonly Uri API_BASE_URI = new Uri("http://api.teleport.org/api/");

        private RestClient _client;

        public TeleportClient()
        {
            _client = new RestClient(API_BASE_URI);
            _client.AddHandler("application/vnd.teleport.v1+json", new JsonDeserializer());
        }

        public async Task<IList<TeleportCitySuggestion>> SearchAsync(string query, CancellationToken token)
        {
            var request = new RestRequest("cities", Method.GET);
            request.AddQueryParameter("search", query);

            var response = await _client.Execute<TeleportCitySearchRootObject>(request, token);
            if (!response.IsSuccess || response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Search failed.");

            if (response.Data?.EmbeddedResults?.CitySearchResults == null)
                throw new Exception("Search failed.");

            var results = response.Data.EmbeddedResults.CitySearchResults;

            return results
                .Select(c => new TeleportCitySuggestion
                {
                    Name           = c.MatchingFullName,
                    AlternateNames = c.MatchingAlternateNames.Select(n => n.Name).ToList(),
                    TeleportUri    = c.Links.CityItem.Uri
                })
                .ToList();
        }

        public async Task<TeleportCity> GetCity(string uri, CancellationToken token)
        {
            var request = new RestRequest(uri, Method.GET);

            var response = await _client.Execute<TeleportGetCityRootObject>(request, token);
            if (!response.IsSuccess || response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Get city failed.");

            if (response.Data?.Location == null)
                throw new Exception("Get city failed.");

            return new TeleportCity
            {
                Name       = response.Data.Name,
                FullName   = response.Data.FullName,
                Latitude   = response.Data.Location.Position.Latitude,
                Longitude  = response.Data.Location.Position.Longitude,
                Population = response.Data.Population,
                TeleportId = response.Data.Id
            };
        }
    }
}
