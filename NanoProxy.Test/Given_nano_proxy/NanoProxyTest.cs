using NanoProxy;
using NanoProxy.Test;
using NUnit.Framework;
using System.Reflection;

namespace Given_nano_proxy
{
    public abstract class NanoProxyTest
    {
        protected NanoProxy<TestClass> Proxy { get; private set; }

        protected object Value { get; private set; }

        protected object OldValue { get; private set; }

        protected PropertyInfo PropertyInfo { get; private set; }

        public virtual void TheTest()
        {
        }

        [SetUp]
        public void TestSetup()
        {
            Proxy = new NanoProxyBuilder().CreateProxy<TestClass>();
            Value = null;
            OldValue = null;
            PropertyInfo = null;

            Proxy.Interceptor = (value, oldValue, property) =>
            {
                Value = value;
                OldValue = oldValue;
                PropertyInfo = property;
            };
            ScenarioSetup();
            TheTest();
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}
