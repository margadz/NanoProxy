using FluentAssertions;
using NanoProxy;
using NanoProxy.Test;
using NUnit.Framework;

namespace Given_nano_proxy_builder
{
    [TestFixture]
    public class when_creating_a_proxy
    {
        [Test]
        public void Should_create_an_instance()
        {
            new NanoProxyBuilder().CreateProxy<TestClass>().WrapedObject.Should().BeAssignableTo(typeof(TestClass));
        }
    }
}
