using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.ViewComponents;

public sealed class Search
    : ViewComponent
{
    public IViewComponentResult Invoke(string? searchString, string resetUrl)
    {
        ArgumentNullException.ThrowIfNull(resetUrl);

        SearchModel viewModel = new(searchString, resetUrl);
        return View(viewModel);
    }
}
