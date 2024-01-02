using BraunauMobil.VeloBasar.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Xan.AspNetCore.Models;
using Xan.AspNetCore.Parameter;
using Xan.Extensions.Collections;

namespace BraunauMobil.VeloBasar.Tests.ViewComponents;

public class PaginationTest
{
    [Theory]
    [VeloAutoData]
    public void ShouldReturnPaginationModel(ListParameter parameter)
    {
        //  Arrange
        Pagination sut = new();
        IPaginatedList items = A.Fake<IPaginatedList>();
        Func<ListParameter, string> toListFunc = X.StrictFake<Func<ListParameter, string>>();

        //  Act
        IViewComponentResult result = sut.Invoke(items, parameter, toListFunc);

        //  Assert
        using (new AssertionScope())
        {
            ViewViewComponentResult view = result.Should().BeOfType<ViewViewComponentResult>().Subject;
            view.ViewData.Should().NotBeNull();
            view.ViewData!.ModelState.IsValid.Should().BeTrue();

            PaginationModel model = view.ViewData.Model.Should().BeOfType<PaginationModel>().Subject;
            model.Items.Should().Be(items);
        }
    }
}
