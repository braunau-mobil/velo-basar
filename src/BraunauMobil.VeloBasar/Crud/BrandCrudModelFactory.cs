using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Rendering;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Xan.AspNetCore.Rendering;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Crud;

public sealed class BrandCrudModelFactory
    : AbstractCrudModelFactory<BrandEntity>
{
    private readonly IVeloHtmlFactory _html;
    private readonly VeloTexts _txt;
    private readonly ISelectListService _selectLists;

    public BrandCrudModelFactory(IVeloHtmlFactory html, VeloTexts txt, ICrudRouter<BrandEntity> router, ISelectListService selectLists)
        : base(router)
    {
        _html = html ?? throw new ArgumentNullException(nameof(html));
        _txt = txt ?? throw new ArgumentNullException(nameof(txt));
        _selectLists = selectLists ?? throw new ArgumentNullException(nameof(selectLists));
    }

    protected override LocalizedString CreateTitle => _txt.CreateBrand;

    protected override LocalizedString EditTitle => _txt.EditBrand;

    protected override LocalizedString ListTitle => _txt.BrandList;

    protected override IHtmlContent CreateEditor(ViewContext viewContext, BrandEntity entity)
    {
        ArgumentNullException.ThrowIfNull(viewContext);
        ArgumentNullException.ThrowIfNull(entity);

        HtmlContentBuilder result = new();
        result.AppendHtml(_html.HiddenInput(nameof(entity.Id), entity.Id));
        result.AppendHtml(_html.TextInputField(nameof(entity.Name), entity.Name, _txt.Name, autoFocus: true));
        result.AppendHtml(_html.SelectField(nameof(entity.State), entity.State, _selectLists.States(), _txt.State));
        return result;
    }

    protected override IHtmlContent CreateTable(ViewContext viewContext, IPaginatedList<CrudItemModel<BrandEntity>> model)
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
                .PercentWidth(70)
                .Title(_txt.Id)
                .For(item => item.Entity.Name)
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
