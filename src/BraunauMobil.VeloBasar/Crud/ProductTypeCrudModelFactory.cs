using BraunauMobil.VeloBasar.Rendering;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Xan.AspNetCore.Rendering;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Crud;

public sealed class ProductTypeCrudModelFactory
    : AbstractCrudModelFactory<ProductTypeEntity>
{
    private readonly IVeloHtmlFactory _html;
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly ISelectListService _selectListSevice;

    public ProductTypeCrudModelFactory(IVeloHtmlFactory html, IStringLocalizer<SharedResources> localizer, ICrudRouter<ProductTypeEntity> router, ISelectListService selectListService)
        : base(router)
    {
        _html = html ?? throw new ArgumentNullException(nameof(html));
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        _selectListSevice = selectListService ?? throw new ArgumentNullException(nameof(selectListService));
    }

    protected override string CreateTitle => VeloTexts.CreateProductType;

    protected override string EditTitle => VeloTexts.EditProductType;

    protected override string ListTitle => VeloTexts.ProductTypeList;

    protected override IHtmlContent CreateEditor(ViewContext viewContext, ProductTypeEntity entity)
    {
        ArgumentNullException.ThrowIfNull(viewContext);
        ArgumentNullException.ThrowIfNull(entity);

        HtmlContentBuilder result = new();
        result.AppendHtml(_html.HiddenInput(nameof(entity.Id), entity.Id));
        result.AppendHtml(_html.TextInputField(nameof(entity.Name), entity.Name, _localizer[VeloTexts.Name], autoFocus: true));
        result.AppendHtml(_html.TextAreaField(nameof(entity.Description), entity.Description, _localizer[VeloTexts.Description]));
        result.AppendHtml(_html.SelectField(nameof(entity.State), entity.State, _selectListSevice.States(), _localizer[VeloTexts.State]));
        return result;
    }

    protected override IHtmlContent CreateTable(ViewContext viewContext, IPaginatedList<CrudItemModel<ProductTypeEntity>> model)
    {
        ArgumentNullException.ThrowIfNull(viewContext);
        ArgumentNullException.ThrowIfNull(model);

        return _html.Table(model)
            .IdColumn()
            .Column(c => c.PercentWidth(30).Title(_localizer[VeloTexts.Name]).For(item => item.Entity.Name))
            .Column(c => c.PercentWidth(70).Title(_localizer[VeloTexts.Description]).For(item => item.Entity.Description))
            .CreatedAtColumn()
            .UpdatedAtColumn()
            .StateColumn()
            .EditLinkColumn(Router)
            .DeleteOrToggleStateLinkColumn(Router)
            .Build();
    }
}
