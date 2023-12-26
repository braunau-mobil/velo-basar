using BraunauMobil.VeloBasar.Parameters;
using BraunauMobil.VeloBasar.Rendering;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;
using Xan.AspNetCore.Rendering;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Crud;

public sealed class SellerCrudModelFactory
    : AbstractCrudModelFactory<SellerEntity, SellerListParameter, ISellerRouter>
{
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly IHtmlHelper _htmlHelper;
    private readonly IVeloHtmlFactory _html;

    public SellerCrudModelFactory(IStringLocalizer<SharedResources> localizer, IHtmlHelper htmlHelper, ISellerRouter router, IVeloHtmlFactory html)
        : base(router)
    {
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        _htmlHelper = htmlHelper ?? throw new ArgumentNullException(nameof(htmlHelper));
        _html = html ?? throw new ArgumentNullException(nameof(html));
    }

    protected override string CreateTitle => _localizer[VeloTexts.CreateSeller];

    protected override string EditTitle => _localizer[VeloTexts.EditSeller];

    protected override string ListTitle => _localizer[VeloTexts.SellerList];

    protected override async Task<IHtmlContent> CreateEditorAsync(ViewContext viewContext, SellerEntity entity)
    {
        ArgumentNullException.ThrowIfNull(viewContext);
        ArgumentNullException.ThrowIfNull(entity);

        if (_htmlHelper is IViewContextAware contextAware)
        {
            contextAware.Contextualize(viewContext);
        }

        HtmlContentBuilder result = new();
        result.AppendHtml(await _htmlHelper.PartialAsync("/Views/Seller/_NameEdit.cshtml", entity));
        result.AppendHtml(await _htmlHelper.PartialAsync("/Views/Seller/_AllButNameEdit.cshtml", entity));
        result.AppendHtml(await _htmlHelper.PartialAsync("/Views/Seller/_CommentEdit.cshtml", entity));

        return await Task.FromResult(result);
    }

    protected override async Task<IHtmlContent> CreateTableAsync(ViewContext viewContext, IPaginatedList<CrudItemModel<SellerEntity>> model)
    {
        ArgumentNullException.ThrowIfNull(viewContext);
        ArgumentNullException.ThrowIfNull(model);

        IHtmlContent table = _html.SellersTable(model.Select(cm => cm.Entity))
                 .Column(c => c.ForLink(item => Router.ToDetails(item.Id), _localizer[VeloTexts.Details]))
                 .Column(c => c.ForLink(item => Router.ToEdit(item.Id), _localizer[VeloTexts.Edit]))
                 .Build();

        return await Task.FromResult(table);
    }
}
