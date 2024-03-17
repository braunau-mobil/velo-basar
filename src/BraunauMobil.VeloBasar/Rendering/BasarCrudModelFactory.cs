using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Xan.AspNetCore.Parameter;
using Xan.AspNetCore.Rendering;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Rendering;

public sealed class BasarCrudModelFactory
    : AbstractCrudModelFactory<BasarEntity, ListParameter, IBasarRouter>
{
    private readonly IVeloHtmlFactory _html;
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly ISelectListService _selectLists;

    public BasarCrudModelFactory(IVeloHtmlFactory html, IStringLocalizer<SharedResources> localizer, IBasarRouter router, ISelectListService selectLists)
        : base(router)
    {
        _html = html ?? throw new ArgumentNullException(nameof(html));
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        _selectLists = selectLists ?? throw new ArgumentNullException(nameof(selectLists));
    }

    protected override string CreateTitle => _localizer[VeloTexts.CreateBasar];

    protected override string EditTitle => _localizer[VeloTexts.EditBasar];

    protected override string ListTitle => _localizer[VeloTexts.BasarList];

    protected override async Task<IHtmlContent> CreateEditorAsync(ViewContext viewContext, BasarEntity entity)
    {
        ArgumentNullException.ThrowIfNull(viewContext);
        ArgumentNullException.ThrowIfNull(entity);

        HtmlContentBuilder result = new();
        result.AppendHtml(_html.HiddenInputs(entity));
        result.AppendHtml(_html.DateInputField(nameof(entity.Date), entity.Date, _localizer[VeloTexts.Date], autoFocus: true));
        result.AppendHtml(_html.TextInputField(nameof(entity.Name), entity.Name, _localizer[VeloTexts.Name]));
        result.AppendHtml(_html.TextInputField(nameof(entity.Location), entity.Location, _localizer[VeloTexts.Location]));
        result.AppendHtml(_html.NumberInputField(nameof(entity.ProductCommissionPercentage), entity.ProductCommissionPercentage, _localizer[VeloTexts.ProductCommissionPercentage]));
        result.AppendHtml(_html.SelectField(nameof(entity.State), entity.State, _selectLists.States(), _localizer[VeloTexts.State]));

        return await Task.FromResult(result);
    }

    protected override async Task<IHtmlContent> CreateTableAsync(ViewContext viewContext, IPaginatedList<CrudItemModel<BasarEntity>> model)
    {
        ArgumentNullException.ThrowIfNull(viewContext);
        ArgumentNullException.ThrowIfNull(model);

        IHtmlContent table = _html.Table(model)
            .IdColumn()
            .Column(c => c.PercentWidth(40).BreakText().Title(_localizer[VeloTexts.Date]).For(item => item.Entity.Date.ToHtmlDateDisplay()))
            .Column(c => 
            {
                c.PercentWidth(60).BreakText().Title("Name & Location").For(item =>
                {
                    HtmlContentBuilder content = new();
                    content.Append(item.Entity.Name);
                    content.AppendHtml("<br/>");
                    content.Append(item.Entity.Location);
                    return content;
                });
            })
            .CreatedAtColumn()
            .UpdatedAtColumn()
            .StateColumn()
            .Column(c => c.ForLink(item => Router.ToDetails(item.Entity.Id), _localizer[VeloTexts.Details]))
            .EditLinkColumn(Router)
            .DeleteOrToggleStateLinkColumn(Router)
            .Build();

        return await Task.FromResult(table);
    }
}
