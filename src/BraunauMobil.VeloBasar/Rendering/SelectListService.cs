using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Xan.AspNetCore.EntityFrameworkCore;
using Xan.AspNetCore.Rendering;

namespace BraunauMobil.VeloBasar.Rendering;

public sealed class SelectListService
    : AbstractSelectListService
    , ISelectListService
{
    private readonly VeloDbContext _db;

    public SelectListService(VeloDbContext db, IStringLocalizer<SharedResources> localizer)
        : base(localizer)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public SelectList AcceptStates(bool includeAll = false)
        => EnumSelectList<AcceptSessionState>(includeAll, VeloTexts.Singular);

    public async Task<ISet<string?>> BrandsAsync()
    {
        string[] brands = await _db.Products.Brands().ToArrayAsync();
        return new HashSet<string?>(brands);
    }

    public async Task<SelectList> BrandsForSelectionAsync(bool includeAll = false)
    {
        List<Tuple<string?, string>> items = new();
        if (includeAll)
        {
            items.Add(new Tuple<string?, string>(null, Localizer[VeloTexts.AllBrand]));
        }
        items.AddRange(await _db.Products.Brands().Select(b => new Tuple<string?, string>(b, b)).ToArrayAsync());
        return new SelectList(items, "Item1", "Item2");
    }

    public async Task<SelectList> CountriesAsync()
    {
        List<Tuple<int?, string>> items = new();
        items.AddRange(await _db.Countries.WhereEnabled().DefaultOrder().Select(c => new Tuple<int?, string>(c.Id, c.Name)).ToArrayAsync());
        return new SelectList(items, "Item1", "Item2");
    }

    public SelectList StorageStates(bool includeAll = false)
        => EnumSelectList<StorageState>(includeAll, VeloTexts.Singular);

    public async Task<SelectList> ProductTypesAsync(bool includeAll = false)
    {
        List<Tuple<int?, string>> items = new();
        if (includeAll)
        {
            items.Add(new Tuple<int?, string>(null, Localizer[VeloTexts.AllProductTypes]));
        }
        items.AddRange(await _db.ProductTypes.WhereEnabled().DefaultOrder().Select(p => new Tuple<int?, string>(p.Id, p.Name)).ToArrayAsync());
        return new SelectList(items, "Item1", "Item2");
    }

    public SelectList TransactionTypes(bool includeAll = false)
        => EnumSelectList<TransactionType>(includeAll, VeloTexts.Singular);

    public SelectList ValueStates(bool includeAll = false)
        => EnumSelectList<ValueState>(includeAll, VeloTexts.Singular);

    public async Task<IDictionary<int, IDictionary<string, string>>> ZipCodeMapAsync()
    {
        Dictionary<int, IDictionary<string, string>> result = new();

        List<ZipCodeEntity> zipCodes = await _db.ZipCodes.ToListAsync();
        foreach (IGrouping<int, ZipCodeEntity> zipGroup in zipCodes.GroupBy(zm => zm.CountryId))
        {
            result.Add(zipGroup.Key, zipGroup.ToDictionary(zm => zm.Zip, zm => zm.City));
        }
        return result;
    }
}
