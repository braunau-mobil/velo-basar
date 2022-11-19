using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

using Xan.AspNetCore.Rendering;

namespace BraunauMobil.VeloBasar.Rendering;

public sealed class DefaultVeloHtmlFactory
     : DefaultBoostrapHtmlFactory
    , IVeloHtmlFactory
{
    public DefaultVeloHtmlFactory(IVeloRouter router, VeloTexts txt)
        : base(router, txt)
    { }

    public IHtmlContent BaseDataStateLink<TEntity>(CrudItemModel<TEntity> item, ICrudRouter router)
        where TEntity : class, ICrudEntity, new()
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentNullException.ThrowIfNull(router);

        string href;
        LocalizedString text;
        if (item.CanDelete)
        {
            href = router.ToDelete(item.Entity.Id);
            text = Txt.Delete;
        }
        else
        {
            if (item.Entity.State == ObjectState.Enabled)
            {
                href = router.ToDisable(item.Entity.Id);
                text = Txt.Disable;
            }
            else
            {
                href = router.ToEnable(item.Entity.Id);
                text = Txt.Enable;
            }
        }
        TagBuilder link = Link(href);
        link.InnerHtml.SetHtmlContent(text);
        return link;
    }    

    public IHtmlContent ProductState(ProductEntity product)
    {
        if (product == null) throw new ArgumentNullException(nameof(product));

        return ProductState(product.StorageState, product.ValueState);
    }

    public IHtmlContent ProductState(StorageState storageState, ValueState valueState)
    {
        LocalizedString text;
        BadgeType type;

        if (storageState == StorageState.NotAccepted && valueState == ValueState.NotSettled)
        {
            text = Txt.Accepting;
            type = BadgeType.Light;
        }
        else if (storageState == StorageState.Available && valueState == ValueState.NotSettled)
        {
            text = Txt.Available;
            type = BadgeType.Success;
        }
        else if (storageState == StorageState.Sold && valueState == ValueState.NotSettled)
        {
            text = Txt.Sold;
            type = BadgeType.Warning;
        }
        else if (storageState == StorageState.Available && valueState == ValueState.Settled)
        {
            text = Txt.PickedUp;
            type = BadgeType.Secondary;
        }
        else if (storageState == StorageState.Sold && valueState == ValueState.Settled)
        {
            text = Txt.Settled;
            type = BadgeType.Secondary;
        }
        else if (storageState == StorageState.Locked)
        {
            text = Txt.Locked;
            type = BadgeType.Danger;
        }
        else if (storageState == StorageState.Lost)
        {
            text = Txt.Lost;
            type = BadgeType.Danger;
        }
        else
        {
            text = Txt.UndefinedProductState;
            type = BadgeType.Danger;
        }

        TagBuilder badge = Badge(type);
        badge.InnerHtml.SetHtmlContent(text);
        return badge;
    }

    public TableBuilder<TModel> Table<TModel>(IEnumerable<TModel> list)
    {
        ArgumentNullException.ThrowIfNull(list);

        return new(list, this);
    }

    public TableBuilder<ProductEntity> ProductsTable(IEnumerable<ProductEntity> products, bool showSum = false, bool showId = false, bool showState = false)
        => ProductsTable(products, item => item, showSum: showSum, showId: showId, showState: showState);

    public TableBuilder<TItemViewModel> ProductsTable<TItemViewModel>(IEnumerable<TItemViewModel> products, Func<TItemViewModel, ProductEntity> getProduct, bool showSum = false, bool showId = false, bool showState = false)
    {
        TableBuilder<TItemViewModel> builder = Table(products);
        if (showId)
        {
            builder.Column()
                .AutoWidth()
                .Title(Txt.Id)
                .DoNotBreak()
                .For(item => getProduct(item).Id);
        }
        builder
            .Column()
                .PercentWidth(10)
                .Title(Txt.Brand)
                .For(item => getProduct(item).Brand.Name)
            .Column()
                .PercentWidth(10)
                .Title(Txt.Type)
                .For(item => getProduct(item).Type.Name)
            .Column()
                .PercentWidth(20)
                .Title(Txt.Color)
                .For(item => getProduct(item).Color)
            .Column()
                .PercentWidth(10)
                .Title(Txt.FrameNumber)
                .For(item => getProduct(item).FrameNumber)
            .Column()
                .PercentWidth(40)
                .Title(Txt.Description)
                .For(item => getProduct(item).Description);

        ColumnBuilder<TItemViewModel> tireSizeBuilder = builder.Column()
            .PercentWidth(5)
            .DoNotBreak()
            .Title(Txt.TireSize);
        if (showSum)
        {
            tireSizeBuilder.Footer()
                .Align(ColumnAlign.Right)
                .For(Txt.Sum);
        }
        tireSizeBuilder
            .For(item => getProduct(item).TireSize);

        ColumnBuilder<TItemViewModel> priceBuilder = builder.Column()
            .PercentWidth(5)
            .DoNotBreak()
            .Align(ColumnAlign.Right)
            .Title(Txt.Price);
        if (showSum)
        {
            priceBuilder.Footer()
                .Align(ColumnAlign.Right)
                .For(products.Select(getProduct).SumPrice().ToHtmlPrice());
        }
        priceBuilder
            .ForPrice(item => getProduct(item).Price);

        if (showState)
        {
            builder.Column()
                .AutoWidth()
                .DoNotBreak()
                .Align(ColumnAlign.Center)
                .For(item => ProductState(getProduct(item)));
        }
        return builder;
    }

    public TableBuilder<SellerEntity> SellersTable(IEnumerable<SellerEntity> sellers)
    {
        ArgumentNullException.ThrowIfNull(sellers);

        return Table(sellers)
            .Column()
                .AutoWidth()
                .Title(Txt.Id)
                .DoNotBreak()
                .For(item => item.Id)
            .Column()
                .PercentWidth(20)
                .Title(Txt.FirstName)
                .For(item => item.FirstName)
            .Column()
                .PercentWidth(10)
                .Title(Txt.LastName)
                .For(item => item.LastName)
            .Column()
                .PercentWidth(35)
                .Title(Txt.Street)
                .For(item => item.Street)
            .Column()
                .PercentWidth(5)
                .Title(Txt.City)
                .For(item => item.City)
            .Column()
                .PercentWidth(5)
                .Title(Txt.ZIP)
                .For(item => item.ZIP)
            .Column()
                .PercentWidth(10)
                .Title(Txt.Country)
                .For(item => item.Country.Name)
            .Column()
                .PercentWidth(5)
                .Title(Txt.ValueState)
                .For(item => Txt.Singular(item.ValueState));
    }

    public TableBuilder<TransactionEntity> TransactionsTable(IEnumerable<TransactionEntity> transactions, bool showType = false, bool showProducts = false)
    {
        ArgumentNullException.ThrowIfNull(transactions);

        TableBuilder<TransactionEntity> builder = Table(transactions);

        builder
            .Column()
                .AutoWidth()
                .Title(Txt.Number)
                .For(item => item.Number)
            .Column()
                .PercentWidth(10)
                .Title(Txt.TimeStamp)
                .ForTimeStamp(item => item.TimeStamp);

        if (showType)
        {
            builder.Column()
                .AutoWidth()
                .Title(Txt.Type)
                .DoNotBreak()
                .For(item =>
                {
                    TagBuilder span = Span();
                    span.AddCssClass($"badge text-bg-{item.Type.ToCssColor()}");
                    span.InnerHtml.SetContent(Txt.Singular(item.Type));
                    return span;
                });
        }

        if (showProducts)
        {
            builder.Column()
                .AutoWidth()
                .Align(ColumnAlign.Center)
                .Title(Txt.ProductCount)
                .For(item => item.Products.Count);
        }

        builder.Column()
            .PercentWidth(90)
            .Title(Txt.Notes)
            .For(item => item.Notes);

        if (showProducts)
        {
            builder.Column()
                .AutoWidth()
                .Title(Txt.Sum)
                .Align(ColumnAlign.Right)
                .DoNotBreak()
                .ForPrice(item => item.GetProductsSum());
        }

        builder
            .LinkColumn()
                .ForLink(item => Router.Transaction.ToDocument(item.Id), Txt.Document, item => item.CanHasDocument)
            .LinkColumn()
                .ForLink(item => Router.Transaction.ToDetails(item.Id), Txt.Details);

        return builder;
    }
}
