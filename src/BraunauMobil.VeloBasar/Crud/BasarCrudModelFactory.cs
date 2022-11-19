using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Rendering;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Xan.AspNetCore.Rendering;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Crud;

public sealed class BasarCrudModelFactory
    : AbstractCrudModelFactory<BasarEntity>
{
    private readonly IVeloHtmlFactory _html;
    private readonly VeloTexts _txt;
    private readonly ISelectListService _selectLists;

    public BasarCrudModelFactory(IVeloHtmlFactory html, VeloTexts txt, ICrudRouter<BasarEntity> router, ISelectListService selectLists)
        : base(router)
    {
        _html = html ?? throw new ArgumentNullException(nameof(html));
        _txt = txt ?? throw new ArgumentNullException(nameof(txt));
        _selectLists = selectLists ?? throw new ArgumentNullException(nameof(selectLists));
    }

    protected override LocalizedString CreateTitle => _txt.CreateBasar;

    protected override LocalizedString EditTitle => _txt.EditBasar;

    protected override LocalizedString ListTitle => _txt.BasarList;

    protected override IHtmlContent CreateEditor(ViewContext viewContext, BasarEntity entity)
    {
        ArgumentNullException.ThrowIfNull(viewContext);
        ArgumentNullException.ThrowIfNull(entity);

        HtmlContentBuilder result = new();
        result.AppendHtml(_html.HiddenInput(nameof(entity.Id), entity.Id));
        result.AppendHtml(_html.DateInputField(nameof(entity.Date), entity.Date, _txt.Date, autoFocus: true));
        result.AppendHtml(_html.TextInputField(nameof(entity.Name), entity.Name, _txt.Name));
        result.AppendHtml(_html.TextInputField(nameof(entity.Location), entity.Location, _txt.Location));
        result.AppendHtml(_html.NumberInputField(nameof(entity.ProductCommissionPercentage), entity.ProductCommissionPercentage, _txt.ProductCommissionPercentage));
        result.AppendHtml(_html.SelectField(nameof(entity.State), entity.State, _selectLists.States(), _txt.State));
        return result;
    }
    
    protected override IHtmlContent CreateTable(ViewContext viewContext, IPaginatedList<CrudItemModel<BasarEntity>> model)
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
                .PercentWidth(10)
                .Title(_txt.Date)
                .For(item => item.Entity.Date.ToHtmlDate())
            .Column()
                .PercentWidth(30)
                .Title(_txt.Name)
                .For(item => item.Entity.Name)
            .Column()
                .PercentWidth(30)
                .Title(_txt.Location)
                .For(item => item.Entity.Location)
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
                .ForLink(item => Router.ToDetails(item.Entity.Id), _txt.Details)
            .LinkColumn()
                .ForLink(item => Router.ToEdit(item.Entity.Id), _txt.Edit)
            .BaseDataStateLinkColumn(_html, Router)
            .Build();
    }
}
