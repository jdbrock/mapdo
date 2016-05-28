using System;
using System.Collections.Generic;

namespace Brock.Services
{
    public static class Services
    {
        // ===========================================================================
        // = Public Properties
        // ===========================================================================

        public static IAssemblyService Assembly { get { return Get<IAssemblyService>(); } }

        // ===========================================================================
        // = Private Fields
        // ===========================================================================

        private static Dictionary<Type, object> _services;

        private static Func<Type, Type> _getBaseTypeFunc { get; set; }
        private static Func<Type, IEnumerable<Type>> _getInterfacesFunc { get; set; }

        // ===========================================================================
        // = Construction
        // ===========================================================================
        
        static Services()
        {
            _services = new Dictionary<Type, Object>();
        }

        public static void Initialize(Func<Type, Type> getBaseTypeFunc, Func<Type, IEnumerable<Type>> getInterfacesFunc)
        {
            _getBaseTypeFunc = getBaseTypeFunc;
            _getInterfacesFunc = getInterfacesFunc;
        }

        // ===========================================================================
        // = Public Methods
        // ===========================================================================
        
        public static T Get<T>()
        {
            return (T)_services[typeof(T)];
        }

        public static void Register(Object inService)
        {
            foreach (var sometype in FindTypesRecursively(inService.GetType()))
                _services.Add(sometype, inService);
        }

        // ===========================================================================
        // = Private Methods
        // ===========================================================================
        
        private static IEnumerable<Type> FindTypesRecursively(Type inType)
        {
            var baseType = _getBaseTypeFunc(inType);

            while (baseType != null && baseType != typeof(Object))
            {
                yield return baseType;
                baseType = _getBaseTypeFunc(baseType);
            }

            var interfaces = _getInterfacesFunc(inType);

            foreach (var item in interfaces)
            {
                yield return item;

                foreach (var subItem in _getInterfacesFunc(item))
                    yield return subItem;
            }
        }
    }
}