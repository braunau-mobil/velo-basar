namespace BraunauMobil.VeloBasar.BusinessLogic;

public interface IDataGeneratorService
{
    void Contextualize(DataGeneratorConfiguration config);

    Task CreateDatabaseAsync();

    Task DropDatabaseAsync();
    
    Task GenerateAsync();

    BasarEntity NextBasar();

    string NextBrand();

    CountryEntity NextCountry();

    ProductEntity NextProduct(string brand, ProductTypeEntity productType, AcceptSessionEntity session);
    
    ProductTypeEntity NextProductType();

    SellerEntity NextSeller(CountryEntity country);
}
