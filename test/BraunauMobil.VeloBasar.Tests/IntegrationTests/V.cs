using BraunauMobil.VeloBasar.Data;
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
        public static ProductTypeEntity Einrad { get; set; }
        
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
                public static ProductEntity Einrad { get; set; }

                public static ProductEntity Stahlross { get; set; }
            }

            public static void AssertStorageStates(VeloDbContext db
                , StorageState einrad
                , StorageState stahlross
                )
            {
                db.AssertProductStorageState(Frodo.Einrad.Id, einrad);
                db.AssertProductStorageState(Frodo.Stahlross.Id, stahlross);
            }

            public static void AssertValueStates(VeloDbContext db
                ,  ValueState einrad
                , ValueState stahlross
                )
            {
                db.AssertProductValueState(Frodo.Einrad.Id, einrad);
                db.AssertProductValueState(Frodo.Stahlross.Id, stahlross);
            }            
        }
    }
}
