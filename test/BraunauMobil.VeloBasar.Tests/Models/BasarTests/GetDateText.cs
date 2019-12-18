using BraunauMobil.VeloBasar.Models;
using System;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Models.BasarTests
{
    public class GetDateText : TestBase
    {
        [Fact]
        public void Empty()
        {
            var basar = new Basar();
            Assert.Equal("01.January.0001", basar.GetDateText());
        }
        [Fact]
        public void FirstContact()
        {
            var basar = new Basar
            {
                Date = new DateTime(2063, 04, 05)
            };
            Assert.Equal("05.April.2063", basar.GetDateText());
        }
    }
}
