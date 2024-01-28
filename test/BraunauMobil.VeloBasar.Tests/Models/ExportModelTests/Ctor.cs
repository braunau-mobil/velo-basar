namespace BraunauMobil.VeloBasar.Tests.Models.ExportModelTests;

public class Ctor
{
    public void CheckDefaults()
    {
        //  Arrange

        //  Act
        ExportModel sut = new();

        //  Assert
        using (new AssertionScope())
        {
            sut.MinPermissionDate.Should().Be(DateOnly.MinValue);
            sut.UseMinPermissionDate.Should().BeFalse();
        }
    }
}
