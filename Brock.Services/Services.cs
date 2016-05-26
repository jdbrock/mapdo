using System;
using System.Collections.Generic;

namespace Brock.Services
{
    public static class Services
    {
        private static Dictionary<Type, Object> RegisteredServices { get; set; }

        public static IAssemblyService Assembly {  get { return Get<IAssemblyService>(); } }

        public static Func<Type, Type> GetBaseTypeFunc { get; set; }
        public static Func<Type, IEnumerable<Type>> GetInterfacesFunc { get; set; }

        static Services()
        {
            RegisteredServices = new Dictionary<Type, Object>();
        }

        public static T Get<T>()
        {
            return (T)RegisteredServices[typeof(T)];
        }

        public static void Register(Object inService)
        {
            foreach (var sometype in FindTypesRecursively(inService.GetType()))
                RegisteredServices.Add(sometype, inService);
        }

        private static IEnumerable<Type> FindTypesRecursively(Type inType)
        {
            var baseType = GetBaseTypeFunc(inType);

            while (baseType != null && baseType != typeof(Object))
            {
                yield return baseType;
                baseType = GetBaseTypeFunc(baseType);
            }

            var interfaces = GetInterfacesFunc(inType);

            foreach (var item in interfaces)
            {
                yield return item;

                foreach (var subItem in GetInterfacesFunc(item))
                    yield return subItem;
            }
        }
    }
}