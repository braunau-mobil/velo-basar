using Microsoft.AspNetCore.Mvc.Rendering;

namespace BraunauMobil.VeloBasar.Rendering;

public interface ISelectListService
{
    SelectList AcceptStates(bool includeAll = false);

    Task<ISet<string?>> BrandsAsync();

    Task<SelectList> BrandsForSelectionAsync(bool includeAll = false);

    Task<SelectList> CountriesAsync();

    SelectList States(bool includeAll = false);

    SelectList StorageStates(bool includeAll = false);

    Task<SelectList> ProductTypesAsync(bool includeAll = false);

    SelectList TransactionTypes(bool includeAll = false);

    SelectList ValueStates(bool includeAll = false);

    Task<IDictionary<int, IDictionary<string, string>>> ZipCodeMapAsync();
}
