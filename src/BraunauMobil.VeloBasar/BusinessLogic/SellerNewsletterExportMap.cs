using CsvHelper.Configuration;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public sealed class SellerNewsletterExportMap : ClassMap<SellerEntity>
{
    public SellerNewsletterExportMap()
    {
        Map(s => s.FirstName).Name(nameof(SellerEntity.FirstName));
        Map(s => s.LastName).Name(nameof(SellerEntity.LastName));
        Map(s => s.Country.Name).Name(nameof(SellerEntity.Country));
        Map(s => s.City).Name(nameof(SellerEntity.City));
        Map(s => s.ZIP).Name(nameof(SellerEntity.ZIP));
        Map(s => s.Street).Name(nameof(SellerEntity.Street));
        Map(s => s.EMail).Name(nameof(SellerEntity.EMail));
        Map(s => s.NewsletterPermissionTimesStamp).Name(nameof(SellerEntity.NewsletterPermissionTimesStamp));
        Map(s => s.UpdatedAt).Name(nameof(SellerEntity.UpdatedAt));
        Map(s => s.CreatedAt).Name(nameof(SellerEntity.CreatedAt));
    }
}
