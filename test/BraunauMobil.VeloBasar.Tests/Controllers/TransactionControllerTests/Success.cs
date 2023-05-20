using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.TransactionControllerTests;

public class Success
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task WithId_ReturnsView(int id)
    {
        //  Arrange
        TransactionEntity transaction = Fixture.BuildTransaction().Create();
        TransactionService.Setup(_ => _.GetAsync(id))
            .ReturnsAsync(transaction);

        //  Act
        IActionResult result = await Sut.Success(id);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.ViewData.ModelState.IsValid.Should().BeTrue();
        
        TransactionSuccessModel model = view.Model.Should().BeOfType<TransactionSuccessModel>().Subject;
        model.Entity.Should().Be(transaction);
        model.OpenDocument.Should().BeTrue();

        TransactionService.Verify(_ => _.GetAsync(id), Times.Once);
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task WithId_AndInvalidAmound_ReturnsView(int id, decimal amountGiven)
    {
        //  Arrange
        TransactionEntity transaction = Fixture.BuildTransaction().Create();
        TransactionService.Setup(_ => _.GetAsync(id, amountGiven))
            .ReturnsAsync(transaction);

        //  Act
        IActionResult result = await Sut.Success(id, amountGiven);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.ViewData.ModelState.IsValid.Should().BeFalse();

        TransactionSuccessModel model = view.Model.Should().BeOfType<TransactionSuccessModel>().Subject;
        model.Entity.Should().Be(transaction);
        model.OpenDocument.Should().BeFalse();
        model.AmountGiven.Should().Be(amountGiven);

        TransactionService.Verify(_ => _.GetAsync(id, amountGiven), Times.Once);
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task WithId_AndValidAmound_ReturnsView(int id, decimal amountGiven)
    {
        //  Arrange
        TransactionEntity transaction = Fixture.BuildTransaction().Create();
        transaction.Change = new ChangeInfo(amountGiven);
        TransactionService.Setup(_ => _.GetAsync(id, amountGiven))
            .ReturnsAsync(transaction);

        //  Act
        IActionResult result = await Sut.Success(id, amountGiven);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.ViewData.ModelState.IsValid.Should().BeTrue();

        TransactionSuccessModel model = view.Model.Should().BeOfType<TransactionSuccessModel>().Subject;
        model.Entity.Should().Be(transaction);
        model.OpenDocument.Should().BeFalse();
        model.AmountGiven.Should().Be(amountGiven);

        TransactionService.Verify(_ => _.GetAsync(id, amountGiven), Times.Once);
        VerifyNoOtherCalls();
    }
}
