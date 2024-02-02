using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Pdf;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.DocumentServiceTests;

public class TestBase
{
    public TestBase()
    {
        Sut = new DocumentService(Factory, ProductLabelGenerator, TransactionDocumentGenerator);
    }

    protected IDocumentModelFactory Factory { get; } = X.StrictFake<IDocumentModelFactory>();

    protected IProductLabelGenerator ProductLabelGenerator { get; } = X.StrictFake<IProductLabelGenerator>();

    protected DocumentService Sut { get; }

    protected ITransactionDocumentGenerator TransactionDocumentGenerator { get; } = X.StrictFake<ITransactionDocumentGenerator>();
}
