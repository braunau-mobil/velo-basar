using Microsoft.AspNetCore.Identity;

namespace BraunauMobil.VeloBasar.Tests.IntegrationTests;

#nullable disable
public static class V
{
    public static IdentityUser AdminUser { get; set; }

    public static BasarEntity FirstBasar { get; set; }

    public static class Countries
    {
        public static CountryEntity Austria { get; set; }
        public static CountryEntity Germany { get; set; }
    }

    public static class ProductTypes
    {
        public static ProductTypeEntity Stahlross { get; set; }
    }

    public static class Sellers
    {
        public static SellerEntity Frodo { get; set; }
    }

    public static class Products
    {
        public static class FirstBasar
        {
            public static class Frodo
            {
                public static ProductEntity Stahlross { get; set; }
            }
        }
    }
}
