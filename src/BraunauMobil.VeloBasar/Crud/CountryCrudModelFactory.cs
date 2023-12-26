using BraunauMobil.VeloBasar.Rendering;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Xan.AspNetCore.Parameter;
using Xan.AspNetCore.Rendering;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Crud;

public sealed class CountryCrudModelFactory
    : AbstractCrudModelFactory<CountryEntity, ListParameter, ICountryRouter>
{
    private readonly IVeloHtmlFactory _html;
    private readonly IStringLocalizer _localizer;
    private readonly ISelectListService _selectLists;

    public CountryCrudModelFactory(IVeloHtmlFactory html, IStringLocalizer<SharedResources> localizer, ICountryRouter router, ISelectListService selectLists)
        : base(router)
    {
        _html = html ?? throw new ArgumentNullException(nameof(html));
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        _selectLists = selectLists ?? throw new ArgumentNullException(nameof(selectLists));
    }

    protected override string CreateTitle => _localizer[VeloTexts.CreateCountry];

    protected override string EditTitle => _localizer[VeloTexts.EditCountry];

    protected override string ListTitle => _localizer[VeloTexts.CountryList];

    protected override async Task<IHtmlContent> CreateEditorAsync(ViewContext viewContext, CountryEntity entity)
    {
        ArgumentNullException.ThrowIfNull(viewContext);
        ArgumentNullException.ThrowIfNull(entity);

        HtmlContentBuilder result = new();
        result.AppendHtml(_html.HiddenInput(nameof(entity.Id), entity.Id));
        result.AppendHtml(_html.TextInputField(nameof(entity.Name), entity.Name, _localizer[VeloTexts.Name], autoFocus: true));
        result.AppendHtml(_html.TextInputField(nameof(entity.Iso3166Alpha3Code), entity.Iso3166Alpha3Code, _localizer[VeloTexts.Iso3166Alpha3Code]));
        result.AppendHtml(_html.SelectField(nameof(entity.State), entity.State, _selectLists.States(), _localizer[VeloTexts.State]));
        
        return await Task.FromResult(result);
    }

    protected override async Task<IHtmlContent> CreateTableAsync(ViewContext viewContext, IPaginatedList<CrudItemModel<CountryEntity>> model)
    {
        ArgumentNullException.ThrowIfNull(viewContext);
        ArgumentNullException.ThrowIfNull(model);

        IHtmlContent table = _html.Table(model)
            .IdColumn()
            .Column(c => c.PercentWidth(70).Title(_localizer[VeloTexts.Name]).For(item => item.Entity.Name))
            .Column(c => c.PercentWidth(30).Title(_localizer[VeloTexts.Iso3166Alpha3Code]).For(item => item.Entity.Iso3166Alpha3Code))
            .CreatedAtColumn()
            .UpdatedAtColumn()
            .StateColumn()
            .EditLinkColumn(Router)
            .DeleteOrToggleStateLinkColumn(Router)
            .Build();

        return await Task.FromResult(table);
    }
}
