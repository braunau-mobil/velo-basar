namespace BraunauMobil.VeloBasar.Models.Entities;

#nullable disable warnings
public sealed class ZipCodeEntity
{
    public string Zip { get; set; }

    public int CountryId { get; set; }

    public string City { get; set; }
}
