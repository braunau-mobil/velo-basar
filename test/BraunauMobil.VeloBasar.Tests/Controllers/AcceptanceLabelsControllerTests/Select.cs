﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AcceptanceLabelsControllerTests;

public class Select
    : TestBase
{
    [Fact]
    public void NoParameters_ReturnsView_WithEmptyAcceptanceLabelsModel()
    {
        // Act
        IActionResult result = Sut.Select();

        // Assert
        result.Should().NotBeNull();
        
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.ViewData.ModelState.IsValid.Should().BeTrue();

        view.Model.Should().NotBeNull();
        view.Model.Should().BeOfType<AcceptanceLabelsModel>();

        VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ValidModel_ReturnsView()
    {
        TransactionEntity transaction = Fixture.BuildTransaction().Create();
        AcceptanceLabelsModel model = Fixture.Create<AcceptanceLabelsModel>();
        model.OpenDocument = false;

        // Arrange
        TransactionService.Setup(ts => ts.FindAsync(model.ActiveBasarId, TransactionType.Acceptance, model.Number))
            .ReturnsAsync(transaction);

        // Act
        IActionResult result = await Sut.Select(model);

        // Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.ViewData.ModelState.IsValid.Should().BeTrue();

        view.Model.Should().Be(model);

        model.OpenDocument.Should().BeTrue();
        model.Id.Should().Be(transaction.Id);

        TransactionService.Verify(ts => ts.FindAsync(model.ActiveBasarId, TransactionType.Acceptance, model.Number), Times.Once);
        VerifyNoOtherCalls();
    }

    [Fact]
    public async Task InvalidValidModel_ReturnsView()
    {
        LocalizedString errorMessage = Fixture.Create<LocalizedString>();
        AcceptanceLabelsModel model = Fixture.Create<AcceptanceLabelsModel>();
        model.OpenDocument = false;

        // Arrange
        Localizer.Setup(_ => _[VeloTexts.NoAcceptanceFound, model.Number])
            .Returns(errorMessage);

        // Act
        IActionResult result = await Sut.Select(model);

        // Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.ViewData.ModelState.IsValid.Should().BeFalse();

        view.Model.Should().NotBe(model);

        AcceptanceLabelsModel resultModel = view.Model.Should().BeOfType<AcceptanceLabelsModel>().Subject;
        resultModel.Number.Should().Be(model.Number);
        resultModel.OpenDocument.Should().BeFalse();
        resultModel.Id.Should().Be(0);
        resultModel.ActiveBasarId.Should().Be(0);

        TransactionService.Verify(_ => _.FindAsync(model.ActiveBasarId, TransactionType.Acceptance, model.Number), Times.Once);
        Localizer.Verify(_ => _[VeloTexts.NoAcceptanceFound, model.Number], Times.Once);
        VerifyNoOtherCalls();
    }
}
