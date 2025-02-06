using BraunauMobil.VeloBasar.Configuration;
using BraunauMobil.VeloBasar.Models.Documents;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Text;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public class DocumentModelFactory
    : IDocumentModelFactory
{
    private const string _poweredByText = "  - powered by https://github.com/braunau-mobil/velo-basar";

    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly PrintSettings _settings;
    private readonly IFormatProvider _formatProvider;

    public DocumentModelFactory(IStringLocalizer<SharedResources> localizer, IOptions<PrintSettings> options, IFormatProvider formatProvider)
    {
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        _formatProvider = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));

        ArgumentNullException.ThrowIfNull(options);
        _settings = options.Value;
    }
    
    public LabelPrintSettings LabelPrintSettings => _settings.Label;

    public AcceptanceDocumentModel CreateAcceptanceModel(TransactionEntity acceptance)
    {
        ArgumentNullException.ThrowIfNull(acceptance);

        if (acceptance.Seller == null)
        {
            throw new InvalidOperationException("Cannot create Acceptance Document without Seller");
        }

        ProductsTableDocumentModel productsTable = CreateProductTable(acceptance.Products.GetProducts(), _localizer[VeloTexts.Price], includeDonationHint: true);

        bool printStatusLinkAndToken = false;
        string statusLink = "";
        if (_settings.Acceptance.StatusLinkFormat is not null)
        {
            printStatusLinkAndToken = true;
            statusLink = string.Format(CultureInfo.InvariantCulture, _settings.Acceptance.StatusLinkFormat, acceptance.Seller.Token);
        }

        return new AcceptanceDocumentModel(
            string.Format(_formatProvider, _settings.Acceptance.TitleFormat, acceptance.Basar.Name, acceptance.Number),
            GetLocationAndDateText(acceptance.Basar),
            GetPageNumberFormat(),
            _poweredByText,
            _settings.PageMargins,
            _settings.Acceptance.SubTitle,
            GetBigAddressText(acceptance.Seller),
            _localizer[VeloTexts.SellerIdShort, acceptance.Seller.Id],
            printStatusLinkAndToken,
            statusLink,
            _settings.Acceptance.TokenTitle,
            acceptance.Seller.Token,
            GetSignatureLine(_settings.Acceptance.SignatureText),
            GetSignatureText(acceptance),
            _settings.QrCodeLengthMillimeters,
            productsTable);       
    }

    public ProductLabelDocumentModel CreateProductLabelModel(ProductEntity product)
    {
        ArgumentNullException.ThrowIfNull(product);

        string? frameNumber = null;
        if (product.FrameNumber is not null)
        {
            frameNumber = _localizer[VeloTexts.FrameNumberLabel, product.FrameNumber];
        }
        string? tireSize = null;
        if (product.TireSize is not null)
        {
            tireSize = _localizer[VeloTexts.TireSizeLabel, product.TireSize];
        }

        return new ProductLabelDocumentModel(
            string.Format(_formatProvider, _settings.Label.TitleFormat, product.Session.Basar.Name),
            $"{product.Brand} - {product.Type.Name}",
            product.Description.Truncate(_settings.Label.MaxDescriptionLength),
            product.Color,
            frameNumber,
            tireSize,
            string.Format(_formatProvider, "{0}", product.Id),
            string.Format(_formatProvider, "{0:C}", product.Price));
    }

    public SaleDocumentModel CreateSaleModel(TransactionEntity sale)
    {
        ArgumentNullException.ThrowIfNull(sale);

        ProductsTableDocumentModel productsTable = CreateProductTable(sale.Products.GetProducts(), _localizer[VeloTexts.Price], true, _settings.Sale.SellerInfoText);

        bool addBanner = false;
        string bannerFilePath = "";
        if (_settings.UseBannerFile)
        {
            addBanner = true;
            bannerFilePath = _settings.BannerFilePath;
        }

        return new SaleDocumentModel(
            string.Format(_formatProvider, _settings.Sale.TitleFormat, sale.Basar.Name, sale.Number),
             GetLocationAndDateText(sale.Basar),
             GetPageNumberFormat(),
            _poweredByText,
            _settings.PageMargins,
            _settings.Sale.SubTitle,
            addBanner,
            bannerFilePath,
            _settings.BannerSubtitle,
            _settings.Website,
            _settings.Sale.HintText,
            _settings.Sale.FooterText,
            productsTable);
    }

    public SettlementDocumentModel CreateSettlementModel(TransactionEntity settlement)
    {
        ArgumentNullException.ThrowIfNull(settlement);

        if (settlement.Seller == null)
        {
            throw new InvalidOperationException("Cannot create Settlement Document without Seller");
        }

        bool addBanner = false;
        string bannerFilePath = "";
        if (_settings.UseBannerFile)
        {
            addBanner = true;
            bannerFilePath = _settings.BannerFilePath;
        }

        decimal payoutAmount = settlement.GetPayoutAmount();
        
        IEnumerable<ProductEntity> payoutProducts = settlement.Products.GetPayoutProducts();
        ProductsTableDocumentModel? payoutProductsTable = null;
        SettlementCommisionSummaryModel? commisisonSummary = null;
        if (payoutProducts.Any())
        {
            payoutProductsTable = CreateProductTable(payoutProducts, _localizer[VeloTexts.SellingPrice]);

            decimal payoutAmountInclCommission = settlement.GetPayoutAmountInclCommission();
            decimal payoutCommissionAmount = settlement.GetPayoutCommissionAmount();
            decimal commissionFactor = settlement.Basar.ProductCommission;

            commisisonSummary = new SettlementCommisionSummaryModel(
                _localizer[VeloTexts.IncomeFromSoldProducts],
                $"{_localizer[VeloTexts.Cost]}:",
                $"{_localizer[VeloTexts.TotalAmount]}:",
                _localizer[VeloTexts.SalesCommision, commissionFactor, payoutAmountInclCommission],
                string.Format(_formatProvider, "{0:C}", payoutAmountInclCommission),
                string.Format(_formatProvider, "{0:C}", payoutCommissionAmount),
                string.Format(_formatProvider, "{0:C}", payoutAmount)
                );
        }

        IEnumerable<ProductEntity> pickupProducts = settlement.Products.GetProductsToPickup();
        ProductsTableDocumentModel? pickupProductsTable = null;
        if (pickupProducts.Any())
        {
            pickupProductsTable = CreateProductTable(pickupProducts, _localizer[VeloTexts.Price]);
        }

        string text = string.Format(CultureInfo.InvariantCulture, _settings.Settlement.BankTransactionTextFormat, settlement.Basar.Name);

        return new SettlementDocumentModel(
            string.Format(_formatProvider, _settings.Settlement.TitleFormat, settlement.Basar.Name, settlement.Number),
            GetLocationAndDateText(settlement.Basar),
            GetPageNumberFormat(),
            _poweredByText,
            _settings.PageMargins,
            addBanner,
            bannerFilePath,
            _settings.BannerSubtitle,
            _settings.Website,
            GetBigAddressText(settlement.Seller),
            _localizer[VeloTexts.SellerIdShort, settlement.Seller.Id],
            commisisonSummary,
            payoutProductsTable,
            _settings.Settlement.SoldTitle,
            pickupProductsTable,
            _settings.Settlement.NotSoldTitle,
            _settings.Settlement.ConfirmationText,
            settlement.NeedsBankingQrCodeOnDocument,
            _settings.QrCodeLengthMillimeters,
            settlement.Seller.EffectiveBankAccountHolder,
            settlement.Seller.IBAN.ToPrettyIBAN(),
            GetEpQrCode(settlement.Seller.EffectiveBankAccountHolder, settlement.Seller.IBAN!, payoutAmount, text),
            GetSignatureLine(_settings.Settlement.SignatureText),
            GetSignatureText(settlement));
    }

    private ProductsTableDocumentModel CreateProductTable(IEnumerable<ProductEntity> products, string priceColumnTitle, bool printSellerInfo = false, string? sellerInfoText = null, bool includeDonationHint = false)
    {
        ProductTableRowDocumentModel[] rows = products.Select(p => GetProductTableRow(p, printSellerInfo, includeDonationHint))
            .ToArray();

        return new ProductsTableDocumentModel(
            _localizer[VeloTexts.Id],
            _localizer[VeloTexts.ProductDescription],
            _localizer[VeloTexts.Size],
            priceColumnTitle,
            $"{_localizer[VeloTexts.Sum]}:",
            _localizer[VeloTexts.ProductCounter, rows.Length],
            string.Format(_formatProvider, "{0:C}", products.SumPrice()),
            sellerInfoText,
            rows);
    }

    private ProductTableRowDocumentModel GetProductTableRow(ProductEntity product, bool printSellerInfo, bool includeDonationHint)
    {
        string? productSellerInfo = null;
        if (printSellerInfo)
        {
            productSellerInfo = $"* {GetSmallAddressText(product.Session.Seller)}";
        }

        return new(
            string.Format(_formatProvider, "{0}", product.Id),
            GetProductInfoText(product, includeDonationHint),
            product.TireSize ?? "",
            string.Format(_formatProvider, "{0:C}", product.Price),
            productSellerInfo
            );
    }

    private string GetProductInfoText(ProductEntity product, bool includeDonationHint)
    {
        StringBuilder sb = new();
        sb.Append(product.Brand).Append(" - ").AppendLine(product.Type.Name);
        sb.Append(product.Description);
        if (!string.IsNullOrEmpty(product.Color)
            || !string.IsNullOrEmpty(product.FrameNumber))
        {
            sb.AppendLine();
            if (!string.IsNullOrEmpty(product.Color))
            {
                sb.Append(' ')
                    .Append(product.Color);
            }
            if (!string.IsNullOrEmpty(product.FrameNumber))
            {
                sb.Append(' ')
                    .Append(product.FrameNumber);
            }
        }
        if (includeDonationHint && product.DonateIfNotSold)
        {
            sb.AppendLine();
            sb.AppendLine(_localizer[VeloTexts.DonateIfNotSoldOnProductTable]);
        }
        return sb.ToString();
    }

    private static string GetSignatureLine(string prefix)
        => $"{prefix} ______________________________";

    private string GetSignatureText(TransactionEntity transaction)
    {
        if (string.IsNullOrEmpty(transaction.Basar.Location))
        {
            return _localizer[VeloTexts.AtDateAndTime, transaction.TimeStamp];
        }
        else
        {
            return _localizer[VeloTexts.AtLocationAndDateAndTime, transaction.Basar.Location, transaction.TimeStamp];
        }
    }

    private string GetPageNumberFormat()
        => _localizer[VeloTexts.PageNumberFromOverall];

    private string GetLocationAndDateText(BasarEntity basar)
        => string.Format(_formatProvider, "{0}, {1:d}", basar.Location, basar.Date);

    private static string GetBigAddressText(SellerEntity seller)
    {
        StringBuilder sb = new();
        sb
            .Append(seller.FirstName).Append(' ').AppendLine(seller.LastName)
            .AppendLine(seller.Street)
            .Append(seller.ZIP).Append(' ').AppendLine(seller.City);
        return sb.ToString();
    }

    private static string GetEpQrCode(string bankAccountHolder, string iban, decimal amount, string text)
    {
        StringBuilder sb = new();
        sb
            .AppendLine("BCD")
            .AppendLine("002")
            .AppendLine("1")
            .AppendLine("SCT")
            .AppendLine()
            .AppendLine(bankAccountHolder)
            .AppendLine(iban)
            .AppendFormat(CultureInfo.InvariantCulture, "EUR{0:#.##}", amount).AppendLine()
            .AppendLine()
            .AppendLine()
            .AppendLine(text)
            .AppendLine()
        ;
        return sb.ToString();
    }

    private static string? GetSmallAddressText(SellerEntity? seller)
    {
        if (seller == null)
        {
            return null;
        }
        return $"{seller.FirstName} {seller.LastName}, {seller.Street}, {seller.ZIP} {seller.City}";
    }
}
