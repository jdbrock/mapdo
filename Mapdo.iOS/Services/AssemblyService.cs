using Brock.Services;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Mapdo.iOS
{
    public class AssemblyService : IAssemblyService
    {
        public List<Assembly> GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies().ToList();
        }
    }
}