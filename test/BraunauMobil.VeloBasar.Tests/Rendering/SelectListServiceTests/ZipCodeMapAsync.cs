using BraunauMobil.VeloBasar.Data;
using Microsoft.EntityFrameworkCore;
using Xan.AspNetCore.Models;

namespace BraunauMobil.VeloBasar.Tests.Rendering.SelectListServiceTests;

public class ZipCodeMapAsync
    : TestBase
{
    [Fact]
    public async Task ShouldReturnMapForAllCountries()
    {
        //  Arrange
        Db.Countries.Add(new CountryEntity
        {
            Iso3166Alpha3Code = "AUT",
            Name = "Österreich",
            State = ObjectState.Enabled
        });
        Db.Countries.Add(new CountryEntity
        {
            Iso3166Alpha3Code = "GER",
            Name = "Deutschland",
            State = ObjectState.Enabled
        });
        await Db.SaveChangesAsync();

        Db.ZipCodes.AddRange(new ZipCollection(await Db.Countries.ToListAsync()));
        await Db.SaveChangesAsync();

        //  Act
        IDictionary<int ,IDictionary<string, string>> result = await Sut.ZipCodeMapAsync();

        //  Assert
        result.Should().SatisfyRespectively(
            austria =>
            {
                austria.Key.Should().Be(1);
                austria.Value.Should().HaveCount(2562);
                austria.Value.Should().Contain(
                    new KeyValuePair<string, string>("5280", "Braunau am Inn"),
                    new KeyValuePair<string, string>("4910", "Ried im Innkreis"),
                    new KeyValuePair<string, string>("4780", "Schärding")
                );
            },
            germany =>
            {
                germany.Key.Should().Be(2);
                germany.Value.Should().HaveCount(8168);
                germany.Value.Should().Contain(
                    new KeyValuePair<string, string>("94032", "Passau"),
                    new KeyValuePair<string, string>("84384", "Wittibreut")
                );
            });
    }
}
