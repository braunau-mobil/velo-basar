namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.SecondBasar;

public class Seller3_CancelOnEnterProducts(TestContext context)
{
    public async Task Run()
    {
        const string expectedTitle = "Acceptance for seller with ID: 3 - Enter products - Velo Basar";

        IHtmlDocument newAcceptanceDocument = await context.HttpClient.NavigateMenuAsync("New Acceptance");
        newAcceptanceDocument.Title.Should().Be("Acceptance - Enter seller - Velo Basar");

        IHtmlFormElement form = newAcceptanceDocument.QueryForm();
        IHtmlButtonElement submitButton = newAcceptanceDocument.QueryButtonByText("Continue");

        IHtmlDocument enterProductsDocument = await context.HttpClient.SendFormAsync(form, submitButton, new Dictionary<string, object>
        {
            { "FirstName", "Amroth" },
            { "LastName", "Gerstenmann" },
            { "CountryId", ID.Countries.Austria },
            { "ZIP", "7872" },
            { "City", "Dwollingen" },
            { "Street", "Concordiaplatz 48" },
            { "PhoneNumber", "161606605" },
            { "EMail", "amroth@gerstenmann.me" },
            { "HasNewsletterPermission", true },
        });
        enterProductsDocument.Title.Should().Be(expectedTitle);

        IHtmlAnchorElement cancelAnchor = enterProductsDocument.QueryAnchorByText("Cancel");
        IHtmlDocument sellerDetailsDocument = await context.HttpClient.GetDocumentAsync(cancelAnchor.Href);
        sellerDetailsDocument.Title.Should().Be("Seller #3 Amroth Gerstenmann - Velo Basar");

        SellerDetailsModel expectedDetails = new (new SellerEntity())
        {
            AcceptedProductCount = 0,
            NotSoldProductCount = 0,
            PickedUpProductCount = 0,
            SettlementAmout = 0,
            SoldProductCount = 0
        };
        await context.AssertSellerDetails(ID.SecondBasar, ID.Sellers.AmrothGerstenmann, expectedDetails);
    }
}
