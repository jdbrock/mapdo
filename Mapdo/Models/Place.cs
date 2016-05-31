using PropertyChanged;
using Realms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using YelpSharp;

namespace Mapdo.Models
{
    public class Place : RealmObject
    {
        // ===========================================================================
        // = Public Properties - Common
        // ===========================================================================

        [Indexed]
        public String Name { get; set; }

        public Double Latitude { get; set; }
        public Double Longitude { get; set; }

        public String Address { get; set; }

        public Boolean IsDone { get; set; }

        // ===========================================================================
        // = Public Properties - Yelp
        // ===========================================================================
        
        public bool HasYelpData { get { return !String.IsNullOrWhiteSpace(YelpId); } }

        public string YelpId { get; set; }
		public string YelpName { get; set; }
		public string YelpImageUrl { get; set; }
		public string YelpUrl { get; set; }
		public string YelpMobileUrl { get; set; }
		public string YelpPhone { get; set; }
		public string YelpDisplayPhone { get; set; }
		//public string[][] YelpCategories { get; set; } // NYI on Realm

        public int YelpReviewCount { get; set; }
        public double YelpRating { get; set; }
		public string YelpRatingImageUrl { get; set; }
		public string YelpRatingImageUrlSmall { get; set; }
		public string YelpRatingImageUrlLarge { get; set; }

		public string YelpSnippetText { get; set; }
		public string YelpSnippetImageUrl { get; set; }

        public bool YelpIsClaimed { get; set; }
        public bool YelpIsClosed { get; set; }

        public double YelpDistance { get; set; }
        public double YelpLocationLatitude { get; set; }
        public double YelpLocationLongitude { get; set; }

        public string YelpLocationDisplayAddress { get; set; }
        //public string[] YelpLocationDisplayAddressRaw { get; set; } // NYI on Realm
        //public string[] YelpLocationAddress { get; set; } // NYI on Realm

        public string YelpLocationCity { get; set; }
        public string YelpLocationCountryCode { get; set; }
        public string YelpLocationCrossStreets { get; set; }
        public double YelpLocationGeoAccuracy { get; set; }
        //public string[] YelpLocationNeighborhoods { get; set; } // NYI on Realm
        public string YelpLocationPostalCode { get; set; }
        public string YelpLocationStateCode { get; set; }

        // TODO: Yelp deals/reviews.
        //public List<YelpDeal> deals { get; set; }
        //public List<YelpReview> reviews { get; set; }

        // ===========================================================================
        // = Public Methods
        // ===========================================================================

        public void SetYelpData(YelpBusiness y)
        {
            YelpId                        = y.id;
            YelpName                      = y.name;
            YelpImageUrl                  = y.image_url;
            YelpUrl                       = y.url;
            YelpMobileUrl                 = y.mobile_url;
            YelpPhone                     = y.phone;
            YelpDisplayPhone              = y.display_phone;
            //YelpCategories                = y.categories; // NYI on Realm

            YelpReviewCount               = y.review_count;
            YelpRating                    = y.rating;
            YelpRatingImageUrl            = y.rating_img_url;
            YelpRatingImageUrlSmall       = y.rating_img_url_small;
            YelpRatingImageUrlLarge       = y.rating_img_url_large;

            YelpSnippetText               = y.snippet_text;
            YelpSnippetImageUrl           = y.snippet_image_url;

            YelpIsClaimed                 = y.is_claimed;
            YelpIsClosed                  = y.is_closed;

            YelpDistance                  = y.distance;
            YelpLocationLatitude          = y.location.coordinate.Latitude;
            YelpLocationLongitude         = y.location.coordinate.Longitude;

            YelpLocationDisplayAddress    = y.location.DisplayAddress;
            //YelpLocationDisplayAddressRaw = y.location.display_address; // NYI on Realm
            //YelpLocationAddress           = y.location.address; // NYI on Realm

            YelpLocationCity              = y.location.city;
            YelpLocationCountryCode       = y.location.country_code;
            YelpLocationCrossStreets      = y.location.cross_streets;
            YelpLocationGeoAccuracy       = y.location.geo_accuracy;
            //YelpLocationNeighborhoods     = y.location.neighborhoods; // NYI on Realm
            YelpLocationPostalCode        = y.location.postal_code;
            YelpLocationStateCode         = y.location.state_code;

            // TODO: Yelp deals/reviews.
            //YelpDeals                   = y.deals;
            //YelpReviews                 = y.reviews;
        }
    }
}
