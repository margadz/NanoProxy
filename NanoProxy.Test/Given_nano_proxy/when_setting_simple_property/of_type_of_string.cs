using FluentAssertions;
using NUnit.Framework;

namespace Given_nano_proxy.when_setting_simple_property
{
    [TestFixture]
    public class of_type_of_string : SimplePropertyTest
    {
        public override void TheTest()
        {
            Proxy.WrapedObject.StringProperty = "stirng";
            Proxy.WrapedObject.StringProperty = "newString";
        }

        [Test]
        public void Should_intercept_setting()
        {
            Value.Should().Be("newString");
        }
    }
}
