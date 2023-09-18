using FluentAssertions;
using NUnit.Framework;

namespace Given_nano_proxy.when_setting_property_of_type_of
{
    [TestFixture]
    public class nullable_integer : NanoProxyTest
    {
        public override void TheTest()
        {
            Proxy.WrapedObject.NullableIntegerProperty = 50;
            Proxy.WrapedObject.NullableIntegerProperty = null;
        }

        [Test]
        public void Should_intercept_new_value()
        {
            Value.Should().Be((int?)null);
        }

        [Test]
        public void Should_intercept_old_value()
        {
            OldValue.Should().Be(50);
        }

        [Test]
        public void Should_intercept_property_type()
        {
            PropertyInfo.Name.Should().Be("NullableIntegerProperty");
        }
    }
}
