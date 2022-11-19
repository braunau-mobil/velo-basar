using BraunauMobil.VeloBasar.BusinessLogic;
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
    private readonly VeloTexts _txt;
    private readonly ISelectListService _selectListSevice;

    public ProductTypeCrudModelFactory(IVeloHtmlFactory html, VeloTexts txt, ICrudRouter<ProductTypeEntity> router, ISelectListService selectListService)
        : base(router)
    {
        _html = html ?? throw new ArgumentNullException(nameof(html));
        _txt = txt ?? throw new ArgumentNullException(nameof(txt));
        _selectListSevice = selectListService ?? throw new ArgumentNullException(nameof(selectListService));
    }

    protected override LocalizedString CreateTitle => _txt.CreateProductType;

    protected override LocalizedString EditTitle => _txt.EditProductType;

    protected override LocalizedString ListTitle => _txt.ProductTypeList;

    protected override IHtmlContent CreateEditor(ViewContext viewContext, ProductTypeEntity entity)
    {
        ArgumentNullException.ThrowIfNull(viewContext);
        ArgumentNullException.ThrowIfNull(entity);

        HtmlContentBuilder result = new();
        result.AppendHtml(_html.HiddenInput(nameof(entity.Id), entity.Id));
        result.AppendHtml(_html.TextInputField(nameof(entity.Name), entity.Name, _txt.Name, autoFocus: true));
        result.AppendHtml(_html.TextAreaField(nameof(entity.Description), entity.Description, _txt.Description));
        result.AppendHtml(_html.SelectField(nameof(entity.State), entity.State, _selectListSevice.States(), _txt.State));
        return result;
    }

    protected override IHtmlContent CreateTable(ViewContext viewContext, IPaginatedList<CrudItemModel<ProductTypeEntity>> model)
    {
        ArgumentNullException.ThrowIfNull(viewContext);
        ArgumentNullException.ThrowIfNull(model);

        return _html.Table(model)
            .Column()
                .AutoWidth()
                .Title(_txt.Id)
                .DoNotBreak()
                .For(item => item.Entity.Id)
            .Column()
                .PercentWidth(20)
                .Title(_txt.Id)
                .For(item => item.Entity.Name)
            .Column()
                .PercentWidth(50)
                .Title(_txt.Description)
                .For(item => item.Entity.Description)
            .Column()
                .PercentWidth(10)
                .Title(_txt.CreatedAt)
                .For(item => item.Entity.CreatedAt.ToHtmlTimeStamp())
            .Column()
                .PercentWidth(10)
                .Title(_txt.UpdatedAt)
                .For(item => item.Entity.UpdatedAt.ToHtmlTimeStamp())
            .Column()
                .PercentWidth(10)
                .Title(_txt.State)
                .For(item => _txt.Singular(item.Entity.State))
            .LinkColumn()
                .ForLink(item => Router.ToEdit(item.Entity.Id), _txt.Edit)
            .BaseDataStateLinkColumn(_html, Router)
            .Build();
    }
}
