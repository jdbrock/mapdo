using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mapdo
{
    [DataContract]
    public class YelpConfiguration
    {
        [DataMember] public String ConsumerKey { get; set; }
        [DataMember] public String ConsumerSecret { get; set; }
        [DataMember] public String AccessToken { get; set; }
        [DataMember] public String AccessTokenSecret { get; set; }
    }
}
