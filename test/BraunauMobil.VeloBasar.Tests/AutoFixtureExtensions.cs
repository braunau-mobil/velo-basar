using AutoFixture.Dsl;
using AutoFixture.Xunit2;
using Xan.AspNetCore.Mvc.Abstractions;
using Xan.Extensions.Collections.Generic;
using Xunit.Sdk;

namespace BraunauMobil.VeloBasar.Tests;

public class VeloFixture
    : Fixture
{
    public VeloFixture()
    {
        Customize<CountryEntity>(CustomizeCountry);
        Customize<ProductTypeEntity>(CustomizeProductType);
        Customize<BasarEntity>(CustomizeBasar);
        Customize<SellerEntity>(CustomizeSeller);
        Customize<AcceptSessionEntity>(CustomizeAcceptSession);
        Customize<FileDataEntity>(CustomizeFileData);
        Customize<ProductEntity>(CustomizeProduct);
        Customize<TransactionEntity>(CustomizeTransaction);

        Customize<ProductDetailsModel>(c => c
            .Without(_ => _.Transactions)
        );
        Customize<SelectSaleModel>(c => c
            .Without(_ => _.Sale)
        );
    }

    public IPostprocessComposer<ProductToTransactionEntity> BuildProductToTransactionEntity(TransactionEntity transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        return Build<ProductToTransactionEntity>()
            .With(_ => _.Transaction, transaction)
            .With(_ => _.TransactionId, transaction.Id);
    }

    public IPaginatedList<T> CreatePaginatedList<T>()
    {
        IList<T> list = this.CreateMany<T>().ToList();
        return CreatePaginatedList(list);
    }

    public IPaginatedList<T> CreatePaginatedList<T>(IList<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        return new PaginatedList<T>(items, this.Create<int>(), this.Create<int>(), this.Create<int>(), this.Create<int>());
    }

    public IPostprocessComposer<AcceptSessionEntity> BuildAcceptSession()
        => CustomizeAcceptSession(Build<AcceptSessionEntity>());

    public IPostprocessComposer<AcceptSessionEntity> BuildAcceptSession(BasarEntity basar)
    {
        ArgumentNullException.ThrowIfNull(basar);

        return BuildAcceptSession()
            .With(_ => _.Basar, basar)
            .With(_ => _.BasarId, basar.Id);
    }

    public IPostprocessComposer<BasarEntity> BuildBasar()
        => CustomizeBasar(Build<BasarEntity>());

    public IPostprocessComposer<CountryEntity> BuildCountry()
        => CustomizeCountry(Build<CountryEntity>());

    public IPostprocessComposer<FileDataEntity> BuildFileData()
        => CustomizeFileData(Build<FileDataEntity>());

    public IPostprocessComposer<ProductEntity> BuildProduct()
        => CustomizeProduct(Build<ProductEntity>());

    public IPostprocessComposer<ProductTypeEntity> BuildProductType()
        => CustomizeProductType(Build<ProductTypeEntity>());

    public IPostprocessComposer<SellerEntity> BuildSeller()
        => CustomizeSeller(Build<SellerEntity>());

    public IPostprocessComposer<SellerDetailsModel> BuildSellerDetailsModel()
        => Build<SellerDetailsModel>();

    public IPostprocessComposer<TransactionEntity> BuildTransaction()
        => CustomizeTransaction(Build<TransactionEntity>());

    public IPostprocessComposer<TransactionEntity> BuildTransaction(BasarEntity basar)
    {
        ArgumentNullException.ThrowIfNull(basar);

        return BuildTransaction()
            .With(_ => _.Basar, basar)
            .With(_ => _.BasarId, basar.Id);
    }

    public IPostprocessComposer<TransactionEntity> BuildTransaction(BasarEntity basar, SellerEntity seller)
    {
        ArgumentNullException.ThrowIfNull(basar);
        ArgumentNullException.ThrowIfNull(seller);

        return BuildTransaction(basar)
            .With(_ => _.Seller, seller)
            .With(_ => _.SellerId, seller.Id);
    }

    public void ExcludeEnumValues<TEnum>(params TEnum[] valuesToExclude)
        where TEnum : struct, Enum
    {
        ArgumentNullException.ThrowIfNull(valuesToExclude);

        Customize<TEnum>(c => new ExcludingEnumBuilder<TEnum>(valuesToExclude));
    }

    private static IPostprocessComposer<AcceptSessionEntity> CustomizeAcceptSession(IPostprocessComposer<AcceptSessionEntity> composer)
       => CustomizeEntity(composer)
            .Without(_ => _.BasarId)
            .Without(_ => _.SellerId);

    private static IPostprocessComposer<BasarEntity> CustomizeBasar(IPostprocessComposer<BasarEntity> composer)
        => CustomizeEntity(composer);

    private static IPostprocessComposer<CountryEntity> CustomizeCountry(IPostprocessComposer<CountryEntity> composer)
        => CustomizeEntity(composer);    

    private static IPostprocessComposer<TEntity> CustomizeEntity<TEntity>(IPostprocessComposer<TEntity> composer)
        where TEntity : IEntity
       => composer.Without(_ => _.Id);

    private static IPostprocessComposer<ProductEntity> CustomizeProduct(IPostprocessComposer<ProductEntity> composer)
        => CustomizeEntity(composer)
            .Without(_ => _.TypeId)
            .Without(_ => _.SessionId);

    private static IPostprocessComposer<ProductTypeEntity> CustomizeProductType(IPostprocessComposer<ProductTypeEntity> composer)
        => CustomizeEntity(composer);

    private static IPostprocessComposer<FileDataEntity> CustomizeFileData(IPostprocessComposer<FileDataEntity> composer)
        => composer.With(_ => _.ContentType, "application/pdf");

    private static IPostprocessComposer<SellerEntity> CustomizeSeller(IPostprocessComposer<SellerEntity> composer)
        => CustomizeEntity(composer)
            .Without(_ => _.CountryId);

    private static IPostprocessComposer<TransactionEntity> CustomizeTransaction(IPostprocessComposer<TransactionEntity> composer)
       => CustomizeEntity(composer)
            .Without(_ => _.BasarId)
            .Without(_ => _.DocumentId)
            .Without(_ => _.ParentTransaction)
            .Without(_ => _.ParentTransactionId)
            .Without(_ => _.SellerId);
}

[DataDiscoverer("AutoFixture.Xunit2.NoPreDiscoveryDataDiscoverer", "AutoFixture.Xunit2")]
public class VeloAutoDataAttribute
    : AutoDataAttribute
{
    public VeloAutoDataAttribute()
        : base(() => new VeloFixture())
    {
    }
}

[DataDiscoverer("AutoFixture.Xunit2.NoPreDiscoveryDataDiscoverer", "AutoFixture.Xunit2")]
public class VeloInlineAutoDataAttribute
    : InlineAutoDataAttribute
{
       public VeloInlineAutoDataAttribute(params object[] values)
        : base(new VeloAutoDataAttribute(), values)
    {
    }
}
