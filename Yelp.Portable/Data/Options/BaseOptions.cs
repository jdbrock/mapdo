using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Yelp.Portable
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseOptions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract Dictionary<string, string> GetParameters();

    }
}
