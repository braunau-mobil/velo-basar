using BraunauMobil.VeloBasar.IntegrationTests.Mockups;

namespace BraunauMobil.VeloBasar.IntegrationTests;

public static class X
{
    public static DateTime FirstContactDay { get; set; } = new DateTime(2063, 04, 05, 11, 22, 33);

    public static ClockMock Clock { get; } = new (() => FirstContactDay);

    public static string Line(this string s, string nextLine = "")
    {
        ArgumentNullException.ThrowIfNull(s);
        ArgumentNullException.ThrowIfNull(nextLine);

        return s + Environment.NewLine + nextLine;
    }    
}
