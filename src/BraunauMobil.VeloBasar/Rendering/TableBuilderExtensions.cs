using Xan.AspNetCore.Rendering;

namespace BraunauMobil.VeloBasar.Rendering;

public static class TableBuilderExtensions
{
    public static TableBuilder<CrudItemModel<TItem>> BaseDataStateLinkColumn<TItem>(this TableBuilder<CrudItemModel<TItem>> builder, IVeloHtmlFactory veloHtml, ICrudRouter router)
        where TItem : class, ICrudEntity, new()
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(veloHtml);
        ArgumentNullException.ThrowIfNull(router);

        return builder.Column()
            .AutoWidth()
            .DoNotBreak()
            .For(item => veloHtml.BaseDataStateLink(item, router));
    }

    public static ColumnBuilder<TItem> LinkColumn<TItem>(this TableBuilder<TItem> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        return builder.Column()
            .AutoWidth()
            .DoNotBreak();
    }
}
