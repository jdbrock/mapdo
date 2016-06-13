using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapdo.Integrations
{
    public class TeleportCitySuggestion
    {
        public string Name { get; set; }
        public IList<string> AlternateNames { get; set; }
        public string TeleportUri { get; set; }
    }
}
