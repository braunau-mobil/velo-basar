using System;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.ExtensionMethodsTests
{
    public class Truncate
    {
        [Theory]
        [InlineData("", "")]
        [InlineData("ab", "ab")]
        [InlineData("abcde", "abcde")]
        [InlineData("abcdefg", "abcde")]
        public void MaxLength5(string input, string expected)
        {
            Assert.Equal(expected, input.Truncate(5));
        }
    }
}
