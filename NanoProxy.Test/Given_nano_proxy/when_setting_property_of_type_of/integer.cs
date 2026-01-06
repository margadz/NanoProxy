using FluentAssertions;
using NUnit.Framework;

namespace Given_nano_proxy.when_setting_property_of_type_of
{
    [TestFixture]
    public class integer : NanoProxyTest
    {
        public override void TheTest()
        {
            Proxy.WrapedObject.IntegerProperty = 50;
            Proxy.WrapedObject.IntegerProperty = 1;
        }

        [Test]
        public void Should_intercept_new_value()
        {
            Value.Should().Be(1);
        }

        [Test]
        public void Should_intercept_old_value()
        {
            OldValue.Should().Be(50);
        }

        [Test]
        public void Should_intercept_property_type()
        {
            PropertyInfo.Name.Should().Be("IntegerProperty");
        }
    }
}
