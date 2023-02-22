using CsvHelper.Configuration;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public sealed class SellerNewsletterExportMap : ClassMap<SellerEntity>
{
    public SellerNewsletterExportMap()
    {
        Map(s => s.Id).Index(0).Name(nameof(SellerEntity.Id));
        Map(s => s.FirstName).Index(1).Name(nameof(SellerEntity.FirstName));
        Map(s => s.LastName).Index(2).Name(nameof(SellerEntity.LastName));
        Map(s => s.Country.Name).Index(3).Name(nameof(SellerEntity.Country));
        Map(s => s.City).Index(4).Name(nameof(SellerEntity.City));
        Map(s => s.ZIP).Index(5).Name(nameof(SellerEntity.ZIP));
        Map(s => s.Street).Index(6).Name(nameof(SellerEntity.Street));
        Map(s => s.EMail).Index(7).Name(nameof(SellerEntity.EMail));
        Map(s => s.NewsletterPermissionTimesStamp).Index(8).Name(nameof(SellerEntity.NewsletterPermissionTimesStamp));
        Map(s => s.UpdatedAt).Index(9).Name(nameof(SellerEntity.UpdatedAt));
        Map(s => s.CreatedAt).Index(10).Name(nameof(SellerEntity.CreatedAt));
    }
}
