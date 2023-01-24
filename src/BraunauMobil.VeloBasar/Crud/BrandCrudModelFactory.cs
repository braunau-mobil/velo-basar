using BraunauMobil.VeloBasar.Rendering;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Xan.AspNetCore.Rendering;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Crud;

public sealed class BrandCrudModelFactory
    : AbstractCrudModelFactory<BrandEntity>
{
    private readonly IVeloHtmlFactory _html;
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly ISelectListService _selectLists;

    public BrandCrudModelFactory(IVeloHtmlFactory html, IStringLocalizer<SharedResources> localizer, ICrudRouter<BrandEntity> router, ISelectListService selectLists)
        : base(router)
    {
        _html = html ?? throw new ArgumentNullException(nameof(html));
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        _selectLists = selectLists ?? throw new ArgumentNullException(nameof(selectLists));
    }

    protected override string CreateTitle => _localizer[VeloTexts.CreateBrand];

    protected override string EditTitle => _localizer[VeloTexts.EditBrand];

    protected override string ListTitle => _localizer[VeloTexts.BrandList];

    protected override IHtmlContent CreateEditor(ViewContext viewContext, BrandEntity entity)
    {
        ArgumentNullException.ThrowIfNull(viewContext);
        ArgumentNullException.ThrowIfNull(entity);

        HtmlContentBuilder result = new();
        result.AppendHtml(_html.HiddenInput(nameof(entity.Id), entity.Id));
        result.AppendHtml(_html.TextInputField(nameof(entity.Name), entity.Name, _localizer[VeloTexts.Name], autoFocus: true));
        result.AppendHtml(_html.SelectField(nameof(entity.State), entity.State, _selectLists.States(), _localizer[VeloTexts.State]));
        return result;
    }

    protected override IHtmlContent CreateTable(ViewContext viewContext, IPaginatedList<CrudItemModel<BrandEntity>> model)
    {
        ArgumentNullException.ThrowIfNull(viewContext);
        ArgumentNullException.ThrowIfNull(model);

        return _html.Table(model)
            .IdColumn()
            .Column(c => c.PercentWidth(100).Title(_localizer[VeloTexts.Name]).For(item => item.Entity.Name))
            .CreatedAtColumn()
            .UpdatedAtColumn()
            .StateColumn()
            .EditLinkColumn(Router)
            .DeleteOrToggleStateLinkColumn(Router)
            .Build();
    }
}
