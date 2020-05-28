using System;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests
{
    public static class DateAssert
    {
        public static void IsDateNow(DateTime? actual)
        {
            Assert.NotNull(actual);
            Assert.Equal(DateTime.Now.Year, actual.Value.Year);
            Assert.Equal(DateTime.Now.Month, actual.Value.Month);
            Assert.Equal(DateTime.Now.Day, actual.Value.Day);
        }
    }
}
