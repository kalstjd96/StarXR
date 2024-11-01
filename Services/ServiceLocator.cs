using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Starxr.SDK.AI.Service
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

        public static void Register<T>(T service) where T : class
        {
            services[typeof(T)] = service;
        }

        public static T Get<T>() where T : class
        {
            return services[typeof(T)] as T;
        }

        public static void Clear()
        {
            services.Clear();
        }
    }
}
