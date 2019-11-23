using BraunauMobil.VeloBasar.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BraunauMobil.VeloBasar.Tests.Models.BasarTests
{
    [TestClass]
    public class GetDateText
    {
        [TestMethod]
        public void Empty()
        {
            var basar = new Basar();
            Assert.AreEqual("01.January.0001", basar.GetDateText());
        }
        [TestMethod]
        public void FirstContact()
        {
            var basar = new Basar
            {
                Date = new DateTime(2063, 04, 05)
            };
            Assert.AreEqual("05.April.2063", basar.GetDateText());
        }
    }
}
