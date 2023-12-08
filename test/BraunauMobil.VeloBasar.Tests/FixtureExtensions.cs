using AutoFixture.Dsl;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests;

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

    public static IPostprocessComposer<SellerEntity> BuildSeller(this Fixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        return fixture.Build<SellerEntity>()
            .Without(_ => _.Id)
            .Without(_ => _.CountryId);
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
            .Without(_ => _.ParentTransactionId)
            .Without(_ => _.DocumentId);
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
}
