using FluentAssertions;
using NUnit.Framework;

namespace Given_nano_proxy.when_setting_property_of_type_of
{
    [TestFixture]
    public class @string : NanoProxyTest
    {
        public override void TheTest()
        {
            Proxy.WrapedObject.StringProperty = "string";
            Proxy.WrapedObject.StringProperty = "newString";
        }

        [Test]
        public void Should_intercept_new_value()
        {
            Value.Should().Be("newString");
        }

        [Test]
        public void Should_intercept_old_value()
        {
            OldValue.Should().Be("string");
        }

        [Test]
        public void Should_intercept_property_type()
        {
            PropertyInfo.Name.Should().Be("StringProperty");
        }
    }
}
