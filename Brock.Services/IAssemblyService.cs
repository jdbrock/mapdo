using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Brock.Services
{
    public interface IAssemblyService
    {
        List<Assembly> GetAssemblies();
    }
}
