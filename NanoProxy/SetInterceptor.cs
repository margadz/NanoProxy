using System.Reflection;

namespace NanoProxy
{
    /// <summary>Proxy property setter interceptor.</summary>
    /// <param name="value">New value.</param>
    /// <param name="oldValue">Old value.</param>
    /// <param name="propertyInfo">Property info.</param>
    public delegate void SetInterceptor(object value, object oldValue, PropertyInfo propertyInfo);

    public delegate void InternalSetInterceptor(object value, object oldValue, string propertyName);
}
