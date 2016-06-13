using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapdo.Integrations
{
    // ===========================================================================
    // = City Search
    // ===========================================================================
    
    public class TeleportCitySearchCityItem
    {
        [JsonProperty("href")]
        public string Uri { get; set; }
    }

    public class CitySearchLinks
    {
        [JsonProperty("city:item")]
        public TeleportCitySearchCityItem CityItem { get; set; }
    }

    public class TeleportCitySearchMatchingAlternateName
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class TeleportCitySearchResult
    {
        [JsonProperty("_links")]
        public CitySearchLinks Links { get; set; }

        [JsonProperty("matching_alternate_names")]
        public List<TeleportCitySearchMatchingAlternateName> MatchingAlternateNames { get; set; }

        [JsonProperty("matching_full_name")]
        public string MatchingFullName { get; set; }
    }

    public class TeleportCitySearchEmbeddedResults
    {
        [JsonProperty("city:search-results")]
        public List<TeleportCitySearchResult> CitySearchResults { get; set; }
    }

    public class TeleportCitySearchRootObject
    {
        [JsonProperty("_embedded")]
        public TeleportCitySearchEmbeddedResults EmbeddedResults { get; set; }
    }

    // ===========================================================================
    // = Get City
    // ===========================================================================

    public class TeleportGetCityNamedLink
    {
        [JsonProperty("href")] public string Uri { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
    }

    public class TeleportGetCityLinks
    {
        [JsonProperty("city:admin1_division")] public TeleportGetCityNamedLink AdminDivision { get; set; }
        [JsonProperty("city:alternate-names")] public TeleportGetCityNamedLink AlternateNames { get; set; }
        [JsonProperty("city:country")]         public TeleportGetCityNamedLink Country { get; set; }
        [JsonProperty("city:timezone")]        public TeleportGetCityNamedLink Timezone { get; set; }
        [JsonProperty("city:urban_area")]      public TeleportGetCityNamedLink UrbanArea { get; set; }
        //public List<Cury> curies { get; set; }
        //public Self self { get; set; }
    }

    public class TeleportGetCityRootObject
    {
        //public Links _links { get; set; }
        [JsonProperty("full_name")] public string FullName { get; set; }
        [JsonProperty("geoname_id")] public int Id { get; set; }
        [JsonProperty("location")] public TeleportGetCityLocation Location { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("population")] public int Population { get; set; }
    }

    public class TeleportGetCityLocation
    {
        [JsonProperty("geohash")] public string Geohash { get; set; }
        [JsonProperty("latlon")] public TeleportGetCityPosition Position { get; set; }
    }

    public class TeleportGetCityPosition
    {
        [JsonProperty("latitude")] public double Latitude { get; set; }
        [JsonProperty("longitude")] public double Longitude { get; set; }
    }
}