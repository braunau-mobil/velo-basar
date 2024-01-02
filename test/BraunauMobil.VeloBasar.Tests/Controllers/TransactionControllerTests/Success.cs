using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.TransactionControllerTests;

public class Success
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task WithId_ReturnsView(int id)
    {
        //  Arrange
        TransactionEntity transaction = Fixture.Create<TransactionEntity>();
        A.CallTo(() => TransactionService.GetAsync(id)).Returns(transaction);

        //  Act
        IActionResult result = await Sut.Success(id);

        //  Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewData.ModelState.IsValid.Should().BeTrue();
        
            TransactionSuccessModel model = view.Model.Should().BeOfType<TransactionSuccessModel>().Subject;
            model.Entity.Should().Be(transaction);
            model.OpenDocument.Should().BeTrue();
        }

        A.CallTo(() => TransactionService.GetAsync(id)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task WithId_AndInvalidAmound_ReturnsView(int id, decimal amountGiven)
    {
        //  Arrange
        TransactionEntity transaction = Fixture.Create< TransactionEntity>();
        A.CallTo(() => TransactionService.GetAsync(id, amountGiven)).Returns(transaction);

        //  Act
        IActionResult result = await Sut.Success(id, amountGiven);

        //  Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewData.ModelState.IsValid.Should().BeFalse();

            TransactionSuccessModel model = view.Model.Should().BeOfType<TransactionSuccessModel>().Subject;
            model.Entity.Should().Be(transaction);
            model.OpenDocument.Should().BeFalse();
            model.AmountGiven.Should().Be(amountGiven);
        }

        A.CallTo(() => TransactionService.GetAsync(id, amountGiven)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task WithId_AndValidAmound_ReturnsView(int id, decimal amountGiven)
    {
        //  Arrange
        TransactionEntity transaction = Fixture.Create<TransactionEntity>();
        transaction.Change = new ChangeInfo(amountGiven);
        A.CallTo(() => TransactionService.GetAsync(id, amountGiven)).Returns(transaction);

        //  Act
        IActionResult result = await Sut.Success(id, amountGiven);

        //  Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewData.ModelState.IsValid.Should().BeTrue();

            TransactionSuccessModel model = view.Model.Should().BeOfType<TransactionSuccessModel>().Subject;
            model.Entity.Should().Be(transaction);
            model.OpenDocument.Should().BeFalse();
            model.AmountGiven.Should().Be(amountGiven);
        }

        A.CallTo(() => TransactionService.GetAsync(id, amountGiven)).MustHaveHappenedOnceExactly();
    }
}
