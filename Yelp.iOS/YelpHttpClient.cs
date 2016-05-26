using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Yelp.Portable;

namespace Yelp.iOS
{
    public class YelpHttpClient : IYelpHttpClient
    {
        private String _rootUri;
        private Options _options;

        public YelpHttpClient(String rootUri, Options options)
        {
            _rootUri = rootUri;
            _options = options;
        }

        /// <summary>
        /// contains all of the oauth magic, makes the http request and returns raw json
        /// </summary>
        /// <param name="parameters">hash array of qs parameters</param>
        /// <returns>plain text json response from the api</returns>
        public Task<T> Query<T>(string area, string id, Dictionary<string, string> parameters)
        {
            // build the url with parameters
            var url = area;
            if (!String.IsNullOrEmpty(id)) url += "/" + Uri.EscapeDataString(id);

            // restsharp FTW!
            var client = new RestClient(_rootUri);
            client.Authenticator = OAuth1Authenticator.ForProtectedResource(_options.ConsumerKey, _options.ConsumerSecret, _options.AccessToken, _options.AccessTokenSecret);
            var request = new RestRequest(url, Method.GET);

            if (parameters != null)
            {
                string[] keys = parameters.Keys.ToArray();
                foreach (string k in keys)
                {
                    request.AddParameter(k, parameters[k]);
                }
            }

            var tcs = new TaskCompletionSource<T>();
            var handle = client.ExecuteAsync(request, response =>
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    tcs.SetResult(default(T));
                }
                else
                {
                    try
                    {
                        T results = JsonConvert.DeserializeObject<T>(response.Content);
                        tcs.SetResult(results);
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                }
            });

            return tcs.Task;
        }
    }
}
