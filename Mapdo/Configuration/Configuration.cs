using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mapdo
{
    [DataContract]
    public class Configuration
    {
        [DataMember] public YelpConfiguration Yelp { get; set; }
        [DataMember] public string HockeyAppId { get; set; }
    }
}
