using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Rendering;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Xan.AspNetCore.Rendering;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Crud;

public sealed class CountryCrudModelFactory
    : AbstractCrudModelFactory<CountryEntity>
{
    private readonly IVeloHtmlFactory _html;
    private readonly VeloTexts _txt;
    private readonly ISelectListService _selectLists;

    public CountryCrudModelFactory(IVeloHtmlFactory html, VeloTexts txt, ICrudRouter<CountryEntity> router, ISelectListService selectLists)
        : base(router)
    {
        _html = html ?? throw new ArgumentNullException(nameof(html));
        _txt = txt ?? throw new ArgumentNullException(nameof(txt));
        _selectLists = selectLists ?? throw new ArgumentNullException(nameof(selectLists));
    }

    protected override LocalizedString CreateTitle => _txt.CreateCountry;

    protected override LocalizedString EditTitle => _txt.EditCountry;

    protected override LocalizedString ListTitle => _txt.CountryList;

    protected override IHtmlContent CreateEditor(ViewContext viewContext, CountryEntity entity)
    {
        ArgumentNullException.ThrowIfNull(viewContext);
        ArgumentNullException.ThrowIfNull(entity);

        HtmlContentBuilder result = new();
        result.AppendHtml(_html.HiddenInput(nameof(entity.Id), entity.Id));
        result.AppendHtml(_html.TextInputField(nameof(entity.Name), entity.Name, _txt.Name, autoFocus: true));
        result.AppendHtml(_html.TextInputField(nameof(entity.Iso3166Alpha3Code), entity.Iso3166Alpha3Code, _txt.Iso3166Alpha3Code));
        result.AppendHtml(_html.SelectField(nameof(entity.State), entity.State, _selectLists.States(), _txt.State));
        return result;
    }

    protected override IHtmlContent CreateTable(ViewContext viewContext, IPaginatedList<CrudItemModel<CountryEntity>> model)
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
                .PercentWidth(50)
                .Title(_txt.Name)
                .For(item => item.Entity.Name)
            .Column()
                .PercentWidth(20)
                .Title(_txt.Iso3166Alpha3Code)
                .For(item => item.Entity.Iso3166Alpha3Code)
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
