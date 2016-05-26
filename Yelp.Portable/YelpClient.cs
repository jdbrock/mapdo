using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Yelp;
using Brock.Services;

namespace Yelp.Portable
{
    /// <summary>
    /// 
    /// </summary>
    public class YelpClient
    {
        //--------------------------------------------------------------------------
        //
        //	Public Methods
        //
        //--------------------------------------------------------------------------

        private async Task<T> Query<T>(string area, string id, Dictionary<string, string> parameters)
        {
            return await Services.Get<IYelpHttpClient>().Query<T>(area, id, parameters);
        }


        /// <summary>
        /// Simple search method to look for a term in a given plain text address
        /// </summary>
        /// <param name="term">what to look for (ex: coffee)</param>
        /// <param name="location">where to look for it (ex: seattle)</param>
        /// <returns>a strongly typed result</returns>
        public Task<SearchResults> Search(string term, string location)
        {
            var result = Query<SearchResults>("search", null, new Dictionary<string, string>
                {
                    { "term", term },
                    { "location", location }
                });

            return result;
        }

        /// <summary>
        /// advanced search based on search options object
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public Task<SearchResults> Search(SearchOptions options)
        {
            var result = Query<SearchResults>("search", null, options.GetParameters());
            return result;
        }

        /// <summary>
        /// search the list of business based on name
        /// </summary>
        /// <param name="name">name of the business you want to get information on</param>
        /// <returns>Business details</returns>
        public Task<Business> GetBusiness(string name)
        {
            var result = Query<Business>("business", name, null);
            return result;
        }

        /// <summary>
        /// search businesses based on phone number
        /// </summary>
        /// <param name="phone">phone number of the business you want to get information on</param>
        /// <returns>List of matching businesses</returns>
        public Task<SearchResults> SearchByPhone(string phone)
        {
            var parameters = new Dictionary<string, string>()
            { 
                {"phone", phone} 
            };
            var result = Query<SearchResults>("phone_search", null, parameters);
            return result;
        }
    }
}
