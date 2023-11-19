using AutoFixture.Dsl;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests
{
    public static class FixtureExtensions
    {
        public static IPostprocessComposer<AcceptSessionEntity> BuildAcceptSessionEntity(this Fixture fixture)
        {
            ArgumentNullException.ThrowIfNull(fixture);

            return fixture.Build<AcceptSessionEntity>()
                .Without(_ => _.Id)
                .Without(_ => _.BasarId)
                .Without(_ => _.SellerId);
        }

        public static IPostprocessComposer<FileDataEntity> BuildFileDataEntity(this Fixture fixture)
        {
            ArgumentNullException.ThrowIfNull(fixture);

            return fixture.Build<FileDataEntity>()
                .With(_ => _.ContentType, "application/pdf");
        }

        public static IPostprocessComposer<ProductEntity> BuildProductEntity(this Fixture fixture)
        {
            ArgumentNullException.ThrowIfNull(fixture);

            return fixture.Build<ProductEntity>()
                .Without(_ => _.Id)
                .Without(_ => _.BrandId)
                .Without(_ => _.TypeId)
                .Without(_ => _.Session)
                .Without(_ => _.SessionId);
        }

        public static IPostprocessComposer<ProductDetailsModel> BuildProductDetailsModel(this Fixture fixture)
        {
            ArgumentNullException.ThrowIfNull(fixture);

            return fixture.Build<ProductDetailsModel>()
                .Without(_ => _.Transactions);
        }

        public static IPostprocessComposer<ProductToTransactionEntity> BuildProductToTransactionEntity(this Fixture fixture, TransactionEntity transaction)
        {
            ArgumentNullException.ThrowIfNull(fixture);
            ArgumentNullException.ThrowIfNull(transaction);

            return fixture.Build<ProductToTransactionEntity>()
                .With(_ => _.Transaction, transaction)
                .With(_ => _.TransactionId, transaction.Id);
        }

        public static IPostprocessComposer<SelectSaleModel> BuildSelectSaleModel(this Fixture fixture)
        {
            ArgumentNullException.ThrowIfNull(fixture);

            return fixture.Build<SelectSaleModel>()
                .Without(_ => _.Sale);
        }

        public static IPostprocessComposer<SellerDetailsModel> BuildSellerDetailsModel(this Fixture fixture)
        {
            ArgumentNullException.ThrowIfNull(fixture);

            return fixture.Build<SellerDetailsModel>()
                .With(_ => _.Transactions, fixture.BuildTransaction().CreateMany().ToList());

        }

        public static IPostprocessComposer<TransactionEntity> BuildTransaction(this Fixture fixture)
        {
            ArgumentNullException.ThrowIfNull(fixture);

            return fixture.Build<TransactionEntity>()
                .Without(_ => _.ParentTransaction)
                .Without(_ => _.ParentTransactionId);
        }

        public static IPaginatedList<T> CreatePaginatedList<T>(this Fixture fixture)
        {
            ArgumentNullException.ThrowIfNull(fixture);

            IList<T> list = fixture.CreateMany<T>().ToList();
            return fixture.CreatePaginatedList(list);
        }

        public static IPaginatedList<T> CreatePaginatedList<T>(this Fixture fixture, IList<T> items)
        {
            ArgumentNullException.ThrowIfNull(fixture);
            ArgumentNullException.ThrowIfNull(items);

            return new PaginatedList<T>(items, fixture.Create<int>(), fixture.Create<int>(), fixture.Create<int>(), fixture.Create<int>());
        }

        public static void ExcludeEnumValues<TEnum>(this IFixture fixture, params TEnum[] valuesToExclude)
        where TEnum : struct, Enum
        {
            ArgumentNullException.ThrowIfNull(fixture);
            ArgumentNullException.ThrowIfNull(valuesToExclude);

            fixture.Customize<TEnum>(c => new ExcludingEnumBuilder<TEnum>(valuesToExclude));
        }

        public static IPostprocessComposer<TransactionEntity> WithBasar(this IPostprocessComposer<TransactionEntity> composer, BasarEntity basar)
        {
            ArgumentNullException.ThrowIfNull(composer);
            ArgumentNullException.ThrowIfNull(basar);

            return composer
                .With(_ => _.Basar, basar)
                .With(_ => _.BasarId, basar.Id);
        }

        // public static IPostprocessComposer<TransactionEntity> WithProducts(this Fixture fixture, BasarEntity basar)
        // {
        //     ArgumentNullException.ThrowIfNull(fixture);

        //     return fixture.Build<TransactionEntity>()
        //         .Without(_ => _.ParentTransaction)
        //         .Without(_ => _.ParentTransactionId)
        //         .Do(transaction =>
        //         {
        //             foreach (ProductToTransactionEntity productToTransactionEntity in fixture.BuildProductToTransactionEntity(transaction).CreateMany())
        //             {
        //                 productToTransactionEntity.Product.Session.Basar = basar;
        //                 productToTransactionEntity.Product.Session.BasarId = basar.Id;

        //                 transaction.Products.Add(productToTransactionEntity);
        //             }
        //         });
        // }

        //public static IPostprocessComposer<Product> BuildProduct(this Fixture fixture, decimal price)
        //{
        //    if (fixture == null) throw new ArgumentNullException(nameof(fixture));

        //    return fixture.Build<Product>()
        //        .With(p => p.Price, price)
        //        .Without(p => p.Id);
        //}
        //public static IPostprocessComposer<Product> BuildProduct(this Fixture fixture, Brand brand, ProductType productType, decimal? price = null)
        //{
        //    if (fixture == null) throw new ArgumentNullException(nameof(fixture));
        //    if (brand == null) throw new ArgumentNullException(nameof(brand));
        //    if (productType == null) throw new ArgumentNullException(nameof(productType));

        //    var builder = fixture.Build<Product>()
        //        .Without(p => p.Id)
        //        .With(p => p.Brand, brand)
        //        .With(p => p.BrandId, brand.Id)
        //        .With(p => p.Type, productType)
        //        .With(p => p.TypeId, productType.Id);
        //    if (price != null)
        //    {
        //        builder = builder.With(p => p.Price, price.Value);
        //    }
        //    return builder;
        //}

        //public static ProductsTransaction CreateAcceptance(this Fixture fixture, IEnumerable<Product> products)
        //{
        //    if (fixture == null) throw new ArgumentNullException(nameof(fixture));

        //    var tx = fixture.Build<ProductsTransaction>()
        //        .With(t => t.Products, products.Select(p => new ProductToTransaction() { Product = p, ProductId = p.Id }).ToArray())
        //        .With(t => t.Type, TransactionType.Acceptance)
        //        .Create();
        //    UpdateProductRelations(tx);
        //    return tx;
        //}
        //public static Seller CreateSeller(this Fixture fixture, Country country)
        //{
        //    if (fixture == null) throw new ArgumentNullException(nameof(fixture));
        //    if (country == null) throw new ArgumentNullException(nameof(country));

        //    return fixture.Build<Seller>()
        //        .With(s => s.Country, country)
        //        .With(s => s.CountryId, country.Id)
        //        .Create();
        //}

        //private static void UpdateProductRelations(ProductsTransaction tx)
        //{
        //    foreach (var product in tx.Products)
        //    {
        //        product.Transaction = tx;
        //        product.TransactionId = tx.Id;
        //    }
        //}
    }
}
