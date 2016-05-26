using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yelp
{
    public interface IYelpHttpClient
    {
        Task<T> Query<T>(string area, string id, Dictionary<string, string> parameters);
    }
}
