using NanoProxy;
using NanoProxy.Test;
using NUnit.Framework;

namespace Given_nano_proxy
{
    public abstract class NanoProxyTest
    {
        protected NanoProxy<TestClass> Proxy { get; private set; }

        public virtual void TheTest()
        {
        }

        [SetUp]
        public void TestSetup()
        {
            Proxy = new NanoProxyBuilder().CreateProxy<TestClass>();
            ScenarioSetup();
            TheTest();
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}
