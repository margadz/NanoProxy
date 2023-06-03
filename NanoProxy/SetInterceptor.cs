using System.Reflection;

namespace NanoProxy
{
    public delegate void SetInterceptor(object value, object oldValue, PropertyInfo propertyInfo);

    internal delegate void InternalSetInterceptor(object value, object oldValue, string propertyName);
}
