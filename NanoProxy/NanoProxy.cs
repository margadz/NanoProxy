using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace NanoProxy
{
    /// <summary>Proxy of given type.</summary>
    /// <typeparam name="T">Type of the proxy.</typeparam>
    public class NanoProxy<T> where T : class, new()
    {
        private static IDictionary<string, PropertyInfo> _propertiesCache = new ConcurrentDictionary<string, PropertyInfo>();

        /// <summary>Gets a wrapped object.</summary>
        public T WrapedObject { get; internal set; }

        /// <summary>Property setter interceptor.</summary>
        public SetInterceptor SetInterceptor { get; set; }

        internal void InternalSetInterceptor(object value, object oldValue, string propertyName)
        {
            PropertyInfo propertyInfo;
            if (!_propertiesCache.TryGetValue(propertyName, out propertyInfo))
            {
                _propertiesCache[propertyName] = propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            }

            SetInterceptor?.Invoke(value, oldValue, propertyInfo);
        }
    }
}
