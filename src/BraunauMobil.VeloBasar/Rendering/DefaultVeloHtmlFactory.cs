using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System.Diagnostics;
using Xan.AspNetCore.Rendering;

namespace BraunauMobil.VeloBasar.Rendering;

public sealed class DefaultVeloHtmlFactory
     : DefaultBoostrapHtmlFactory
    , IVeloHtmlFactory
{
    private readonly IVeloRouter _router;

    public DefaultVeloHtmlFactory(IVeloRouter router, IStringLocalizer<SharedResources> localizer)
        : base(localizer)
    {
        _router = router ?? throw new ArgumentNullException(nameof(router));
    }

    public TagBuilder Alert(MessageType type, IHtmlContent content)
    {
        TagBuilder div = Div();
        div.AddCssClass("alert");
        div.AddCssClass(type.ToCss());
        div.Attributes.Add("role", "alert");
        div.InnerHtml.SetHtmlContent(content);
        return div;
    }


    public TagBuilder Alert(MessageType type, string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        TagBuilder p = Paragraph();
        p.InnerHtml.SetHtmlContent(text);

        return Alert(type, p);
    }

    public TagBuilder Alert(MessageType type, string title, string text)
    {
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(text);

        TagBuilder heading = Heading(4);
        heading.AddCssClass("alert-heading");
        heading.InnerHtml.SetHtmlContent(title);

        TagBuilder p = Paragraph();
        p.InnerHtml.SetHtmlContent(text);

        HtmlContentBuilder content = new();
        content.AppendHtml(heading);
        content.AppendHtml(p);

        return Alert(type, content);
    }

    public TagBuilder Badge(BadgeType type)
    {
        string css = $"badge rounded-pill {type.ToCss()}";
        TagBuilder span = Span();
        span.AddCssClass(css);
        return span;
    }

    public HtmlContentBuilder HiddenInputs(AbstractEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        HtmlContentBuilder builder = new();
        builder.AppendHtml(this.HiddenIEntityInput(entity));
        if (entity is IHasTimestamps hasTimestamps)
        {
            builder.AppendHtml(this.HiddenIHasTimestampsInput(hasTimestamps));
        }

        return builder;
    }

    public IHtmlContent ProductDonateableBadge(ProductEntity product)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (product.DonateIfNotSold)
        {
            TagBuilder donateBadge = Badge(BadgeType.Info);
            donateBadge.InnerHtml.SetHtmlContent(Localizer[VeloTexts.Donateable]);
            return donateBadge;
        }

        return new HtmlString("");
    }

    public IHtmlContent ProductInfoBadges(ProductEntity product)
    {
        ArgumentNullException.ThrowIfNull(product);

        HtmlContentBuilder badges = new();
        badges.AppendHtml(ProductStateBadge(product));
        TagBuilder br = new("br")
        {
            TagRenderMode = TagRenderMode.StartTag
        };
        badges.AppendHtml(br);
        badges.AppendHtml(ProductDonateableBadge(product));

        return badges;
    }

    public IHtmlContent ProductStateBadge(ProductEntity product)
    {
        ArgumentNullException.ThrowIfNull(product);

        string text;
        BadgeType type;
        StorageState storageState = product.StorageState;
        ValueState valueState = product.ValueState;

        if (storageState == StorageState.NotAccepted && valueState == ValueState.NotSettled)
        {
            text = VeloTexts.Accepting;
            type = BadgeType.Light;
        }
        else if (valueState == ValueState.Settled)
        {
            type = BadgeType.Secondary;
            
            if (storageState == StorageState.Available
            || storageState == StorageState.Locked)
            {
                text = VeloTexts.PickedUp;
            }
            else if (storageState == StorageState.Sold
                    || storageState == StorageState.Lost)
            {
                text = VeloTexts.Settled;
            }
            else
            {
                text = VeloTexts.UndefinedProductState;
                type = BadgeType.Danger;
            }
        }
        else if (valueState == ValueState.NotSettled)
        {
            if (storageState == StorageState.Available)
            {
                text = VeloTexts.Available;
                type = BadgeType.Success;
            }
            else if (storageState == StorageState.Sold)
            {
                text = VeloTexts.Sold;
                type = BadgeType.Warning;
            }
            else if (storageState == StorageState.Locked)
            {
                text = VeloTexts.Locked;
                type = BadgeType.Danger;
            }
            else if (storageState == StorageState.Lost)
            {
                text = VeloTexts.Lost;
                type = BadgeType.Danger;
            }
            else
            {
                text = VeloTexts.UndefinedProductState;
                type = BadgeType.Danger;
            }
        }
        else
        {
            text = VeloTexts.UndefinedProductState;
            type = BadgeType.Danger;
        }

        TagBuilder badge = Badge(type);
        badge.InnerHtml.SetHtmlContent(Localizer[text]);

        return badge;
    }

    public TableBuilder<ProductEntity> ProductsTable(IEnumerable<ProductEntity> products, bool showSum = false, bool showId = false, bool showState = false)
        => ProductsTable(products, item => item, showSum: showSum, showId: showId, showState: showState);

    public TableBuilder<TItemViewModel> ProductsTable<TItemViewModel>(IEnumerable<TItemViewModel> products, Func<TItemViewModel, ProductEntity> getProduct, bool showSum = false, bool showId = false, bool showState = false)
    {
        TableBuilder<TItemViewModel> builder = Table(products);
        if (showId)
        {
            builder.IdColumn(item => getProduct(item).Id);
        }
        builder
            .Column(c =>
            {
                c.PercentWidth(20).BreakText().Title(Localizer[VeloTexts.BrandAndType]).For(item =>
                {
                    ProductEntity product = getProduct(item);
                    HtmlContentBuilder content = new();
                    content.Append(product.Brand);
                    content.AppendHtml("<br/>");
                    content.Append(product.Type.Name);
                    return content;
                });
            })
            .Column(c =>
            {
                c.PercentWidth(75).BreakText().Title(Localizer[VeloTexts.Description]).For(item =>
                {
                    ProductEntity product = getProduct(item);
                    HtmlContentBuilder content = new();
                    if (product.Color is not null)
                    {
                        content.Append(product.Color);
                    }
                    if (product.Color is not null && product.TireSize is not null)
                    {
                        content.Append(" - ");
                    }
                    if (product.TireSize is not null)
                    {
                        content.Append(product.TireSize);
                    }
                    content.AppendHtml("<br/>");
                    if (product.FrameNumber is not null)
                    {
                        content.Append(product.FrameNumber);
                        content.Append(" - ");
                    }
                    content.Append(product.Description);
                    return content;
                });
                if (showSum)
                {
                    c.Footer(f => f.Align(ColumnAlign.Right).For(Localizer[VeloTexts.Sum]));
                }
            })
            .Column(c =>
            {
                c.PercentWidth(5).Align(ColumnAlign.Right).Title(Localizer[VeloTexts.Price]).ForPrice(item => getProduct(item).Price);
                if (showSum)
                {
                    IHtmlContent sum = products.Select(getProduct).SumPrice().ToHtmlPriceDisplay();
                    c.Footer(f => f.Align(ColumnAlign.Right).For(sum));
                }
            });
        if (showState)
        {
            builder.Column(c => c.AutoWidth().Align(ColumnAlign.Center).For(item => ProductInfoBadges(getProduct(item))));
        }
        return builder;
    }

    public TableBuilder<SellerEntity> SellersTable(IEnumerable<SellerEntity> sellers)
    {
        ArgumentNullException.ThrowIfNull(sellers);

        return Table(sellers)
            .IdColumn()
            .Column(c => c.PercentWidth(17).BreakText().Title(Localizer[VeloTexts.FirstName]).For(item => item.FirstName))
            .Column(c => c.PercentWidth(18).BreakText().Title(Localizer[VeloTexts.LastName]).For(item => item.LastName))
            .Column(c =>
            {
                c.PercentWidth(65).Title(Localizer[VeloTexts.Address]).For(item =>
                {
                    HtmlContentBuilder content = new();
                    content.Append(item.Street);
                    content.AppendHtml("<br/>");
                    content.Append($"{item.ZIP} {item.City} {item.Country.Name}");
                    return content;
                });
            })
            .Column(c => c.AutoWidth().Align(ColumnAlign.Center).For(item => SellerStateBadges(item)));
    }

    public IHtmlContent SellerStateBadges(SellerEntity seller)
    {
        ArgumentNullException.ThrowIfNull(seller);

        BadgeType valueStateBadgeType;
        BadgeType settlementTypeBadgeType;
        if (seller.ValueState == ValueState.Settled)
        {
            valueStateBadgeType = BadgeType.Secondary;
            settlementTypeBadgeType = BadgeType.Secondary;
        }
        else if (seller.ValueState == ValueState.NotSettled)
        {
            valueStateBadgeType = BadgeType.Primary;
            if (seller.SettlementType == SellerSettlementType.Remote)
            {
                settlementTypeBadgeType = BadgeType.Success;
            }
            else if (seller.SettlementType == SellerSettlementType.OnSite)
            {
                settlementTypeBadgeType = BadgeType.Warning;
            }
            else
            {
                throw new UnreachableException();
            }
        }
        else
        {
            throw new UnreachableException();
        }

        HtmlContentBuilder badges = new();
        TagBuilder valueStateBadge = Badge(valueStateBadgeType);
        valueStateBadge.InnerHtml.SetHtmlContent(Localizer[VeloTexts.Singular(seller.ValueState)]);
        badges.AppendHtml(valueStateBadge);
        TagBuilder br = new("br")
        {
            TagRenderMode = TagRenderMode.StartTag
        };
        badges.AppendHtml(br);

        TagBuilder settlementTypeBadge = Badge(settlementTypeBadgeType);
        settlementTypeBadge.InnerHtml.SetHtmlContent(Localizer[VeloTexts.Singular(seller.SettlementType)]);
        badges.AppendHtml(settlementTypeBadge);
        return badges;
    }

    public TableBuilder<TransactionEntity> TransactionsTable(IEnumerable<TransactionEntity> transactions, bool showType = false, bool showProducts = false)
    {
        ArgumentNullException.ThrowIfNull(transactions);

        TableBuilder<TransactionEntity> builder = Table(transactions);

        builder
            .Column(c => c.AutoWidth().Title(Localizer[VeloTexts.Number]).For(item => item.Number))
            .Column(c => c.PercentWidth(10).Title(Localizer[VeloTexts.TimeStamp]).For(item => item.TimeStamp));

        if (showType)
        {
            builder.Column(c => c.AutoWidth().Title(Localizer[VeloTexts.Type]).For(item =>
            {
                TagBuilder span = Span();
                span.AddCssClass($"badge text-bg-{item.Type.ToCssColor()}");
                span.InnerHtml.SetContent(Localizer[VeloTexts.Singular(item.Type)]);
                return span;
            }));
        }

        if (showProducts)
        {
            builder.Column(c => c.AutoWidth().Title(Localizer[VeloTexts.ProductCount]).Align(ColumnAlign.Center).For(item => item.Products.Count));
        }

        builder.Column(c => c.PercentWidth(90).BreakText().Title(Localizer[VeloTexts.Notes]).For(item => item.Notes));

        if (showProducts)
        {
            builder.Column(c => c.AutoWidth().Title(Localizer[VeloTexts.ProductsValue]).Align(ColumnAlign.Right).ForPrice(item => item.GetProductsValue()));
        }

        builder
            .Column(c => c.AutoWidth().ForLink(item => _router.Transaction.ToDocument(item.Id), Localizer[VeloTexts.Document], item => item.CanHasDocument))
            .Column(c => c.AutoWidth().ForLink(item => _router.Transaction.ToDetails(item.Id), Localizer[VeloTexts.Details]));

        return builder;
    }
}
