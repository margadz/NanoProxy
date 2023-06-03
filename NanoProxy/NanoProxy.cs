using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace NanoProxy
{
    public class NanoProxy<T> where T : class, new()
    {
        private IDictionary<string, PropertyInfo> _propertiesCache = new ConcurrentDictionary<string, PropertyInfo>();

        public T WrapedObject { get; internal set; }

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
