namespace BraunauMobil.VeloBasar.BusinessLogic;

public interface IDataGeneratorService
{
    void Contextualize(DataGeneratorConfiguration config);
    
    Task GenerateAsync();

    BasarEntity NextBasar();

    BrandEntity NextBrand();

    CountryEntity NextCountry();

    ProductEntity NextProduct(BrandEntity brand, ProductTypeEntity productType, AcceptSessionEntity session);
    
    ProductTypeEntity NextProductType();

    SellerEntity NextSeller(CountryEntity country);
}
