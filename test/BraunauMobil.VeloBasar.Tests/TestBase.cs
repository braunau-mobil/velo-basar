using System.Globalization;

namespace BraunauMobil.VeloBasar.Tests
{
    public class TestBase
    {
        public TestBase()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
        }
    }
}
