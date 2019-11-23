using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace BraunauMobil.VeloBasar.Tests
{
    [TestClass]
    public static class AssemblySetup
    {
        [AssemblyInitialize]
        public static void Init(TestContext testContext)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
        }
    }
}
