using Microsoft.AspNetCore.Mvc;
using Xan.AspNetCore.Models;
using Xan.AspNetCore.Parameter;
using Xan.Extensions.Collections;

namespace BraunauMobil.VeloBasar.ViewComponents;

public sealed class Pagination
    : ViewComponent
{
    public IViewComponentResult Invoke(IPaginatedList items, ListParameter currentParameter, Func<ListParameter, string> toList)
    {
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(toList);

        PaginationModel viewModel = new(items, currentParameter, toList);
        return View(viewModel);
    }
}
