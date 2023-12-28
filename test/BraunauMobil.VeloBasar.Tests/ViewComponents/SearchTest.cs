using BraunauMobil.VeloBasar.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace BraunauMobil.VeloBasar.Tests.ViewComponents;

public class SearchTest
{
    [Theory]
    [InlineAutoData(null)]
    [AutoData]
    public void ShouldReturnSearchViewModel(string? searchString, string resetUrl)
    {
        //  Arrange
        Search sut = new();

        //  Act
        IViewComponentResult result = sut.Invoke(searchString, resetUrl);

        //  Assert
        using (new AssertionScope())
        {
            ViewViewComponentResult view = result.Should().BeOfType<ViewViewComponentResult>().Subject;
            view.ViewData.Should().NotBeNull();
            view.ViewData!.ModelState.IsValid.Should().BeTrue();
            
            SearchModel model = view.ViewData.Model.Should().BeOfType<SearchModel>().Subject;
            model.SearchString.Should().Be(searchString);
            model.ResetUrl.Should().Be(resetUrl);
        }
    }
}
