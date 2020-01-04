using Xunit;

namespace BraunauMobil.VeloBasar.Tests.IbanAttributeTests
{
    /// <summary>
    /// All IBANs used in this test are random generated and do not represent real bank accounts of real people.
    /// </summary>
    public class IsValid
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("AT36 3200 0746 4669 1845")]
        [InlineData("AT70 1400 0122 1166 8598")]
        [InlineData("AT581947027846184399")]
        [InlineData("DE35500105173333512138")]
        [InlineData("DE23 5001 0517 6767 6479 74")]
        [InlineData("DE29 5001 0517 1339 2397 76")]
        public void Valid(object input)
        {
            var attribute = new IbanAttribute();
            Assert.True(attribute.IsValid(input));
        }

        [Theory]
        [InlineData("AT1")]
        [InlineData("AT12 3456 7891 2345")]
        [InlineData("AT123456789987654321456875324892248")]
        [InlineData("I am not an iban")]
        public void Error(object input)
        {
            var attribute = new IbanAttribute();
            Assert.False(attribute.IsValid(input));
        }
    }
}
