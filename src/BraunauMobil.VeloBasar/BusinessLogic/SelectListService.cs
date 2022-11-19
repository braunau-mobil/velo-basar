using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Xan.AspNetCore.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public sealed class SelectListService
    : ISelectListService
{
    private readonly VeloDbContext _db;
    private readonly VeloTexts _txt;

    public SelectListService(VeloDbContext db, VeloTexts txt)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _txt = txt ?? throw new ArgumentNullException(nameof(txt));
    }

    public SelectList AcceptStates(bool includeAll = false)
        => EnumSelectList<AcceptSessionState>(includeAll, _txt.Singular);

    public async Task<SelectList> BrandsAsync(bool includeAll = false)
    {
        List<Tuple<int?, string>> items = new();
        if (includeAll)
        {
            items.Add(new Tuple<int?, string>(null, _txt.AllBrand));
        }
        items.AddRange(await _db.Brands.WhereEnabled().DefaultOrder().Select(b => new Tuple<int?, string>(b.Id, b.Name)).ToArrayAsync());
        return new SelectList(items, "Item1", "Item2");
    }

    public async Task<SelectList> CountriesAsync()
    {
        List<Tuple<int?, string>> items = new();
        items.AddRange(await _db.Countries.WhereEnabled().DefaultOrder().Select(b => new Tuple<int?, string>(b.Id, b.Name)).ToArrayAsync());
        return new SelectList(items, "Item1", "Item2");
    }

    public SelectList States(bool includeAll = false)
        => EnumSelectList<ObjectState>(includeAll, _txt.Singular);

    public SelectList StorageStates(bool includeAll = false)
        => EnumSelectList<StorageState>(includeAll, _txt.Singular);

    public async Task<SelectList> ProductTypesAsync(bool includeAll = false)
    {
        List<Tuple<int?, string>> items = new();
        if (includeAll)
        {
            items.Add(new Tuple<int?, string>(null, _txt.AllProductType));
        }
        items.AddRange(await _db.ProductTypes.WhereEnabled().DefaultOrder().Select(b => new Tuple<int?, string>(b.Id, b.Name)).ToArrayAsync());
        return new SelectList(items, "Item1", "Item2");
    }

    public SelectList TransactionTypes(bool includeAll = false)
        => EnumSelectList<TransactionType>(includeAll, _txt.Singular);

    public SelectList ValueStates(bool includeAll = false)
        => EnumSelectList<ValueState>(includeAll, _txt.Singular);

    public async Task<IDictionary<int, IDictionary<string, string>>> ZipCodeMapAsync()
    {
        Dictionary<int, IDictionary<string, string>>  result = new ();

        List<ZipCodeEntity> zipCodes = await _db.ZipCodes.ToListAsync();
        foreach (IGrouping<int, ZipCodeEntity> zipGroup in zipCodes.GroupBy(zm => zm.CountryId))
        {
            result.Add(zipGroup.Key, zipGroup.ToDictionary(zm => zm.Zip, zm => zm.City));
        }
        return result;
    }

    private static SelectList EnumSelectList<TEnum>(bool includeAll, Func<TEnum?, LocalizedString> getDisplayText)
        where TEnum : struct, Enum
    {
        List<Tuple<TEnum?, string>> items = new();
        if (includeAll)
        {
            items.Add(new Tuple<TEnum?, string>(null, getDisplayText(null)));
        }

        foreach (TEnum value in Enum.GetValues<TEnum>())
        {
            items.Add(new Tuple<TEnum?, string>(value, getDisplayText(value)));
        }

        return new SelectList(items, "Item1", "Item2");
    }
}
