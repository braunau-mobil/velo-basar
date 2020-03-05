using System;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests
{
    public static class DateAssert
    {
        public static void IsDateNow(DateTime actual)
        {
            Assert.Equal(DateTime.Now.Year, actual.Year);
            Assert.Equal(DateTime.Now.Month, actual.Month);
            Assert.Equal(DateTime.Now.Day, actual.Day);
        }
    }
}
