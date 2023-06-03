using System.Reflection;

namespace Given_nano_proxy.when_setting_simple_property
{
    public abstract class SimplePropertyTest : NanoProxyTest
    {
        protected object Value { get; private set; }

        protected object OldValue { get; private set; }

        protected PropertyInfo PropertyInfo { get; private set; }

        protected override void ScenarioSetup()
        {
            Value = null;
            OldValue = null;
            PropertyInfo = null;

            Proxy.SetInterceptor = (value, oldValue, property) =>
            {
                Value = value;
                OldValue = oldValue;
                PropertyInfo = PropertyInfo;
            };
        }
    }
}
