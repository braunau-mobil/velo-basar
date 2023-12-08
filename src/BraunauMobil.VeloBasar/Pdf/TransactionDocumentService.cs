using System.Globalization;
using System.IO;
using System.Text;
using BraunauMobil.VeloBasar.Configuration;
using iText.Barcodes;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Pdf;

public sealed class TransactionDocumentService
    : ITransactionDocumentService
{
    private readonly PdfGenerator _pdf;
    private readonly PrintSettings _settings;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public TransactionDocumentService(PdfGenerator pdf, IOptions<PrintSettings> options, IStringLocalizer<SharedResources> localizer)
    {
        _pdf = pdf ?? throw new ArgumentNullException(nameof(pdf));
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));

        ArgumentNullException.ThrowIfNull(options);
        _settings = options.Value;
    }

    public async Task<byte[]> CreateAsync(TransactionEntity transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        if (transaction.Type == TransactionType.Acceptance)
        {
            return await Task.FromResult(CreateAcceptance(transaction));
        }
        else if (transaction.Type == TransactionType.Sale)
        {
            return await Task.FromResult(CreateSale(transaction));
        }
        else if (transaction.Type == TransactionType.Settlement)
        {
            return await Task.FromResult(CreateSettlement(transaction));
        }

        throw new InvalidOperationException($"Cannot generate transaction document for: {transaction.Type}");
    }

    private byte[] CreateAcceptance(TransactionEntity acceptance)
    {
        if (acceptance.Seller == null)
        {
            throw new InvalidOperationException("Cannot create Acceptance Document without Seller");
        }

        return CreateTransactionPdf((pdfDoc, doc) =>
        {
            _pdf.AddHeader(doc, GetBigAddressText(acceptance.Seller), GetLocationAndDateText(acceptance.Basar), _localizer[VeloTexts.SellerIdShort, acceptance.Seller.Id]);

            _pdf.AddTitle(doc, string.Format(CultureInfo.CurrentCulture, _settings.Acceptance.TitleFormat, acceptance.Basar.Name, acceptance.Number));
            _pdf.AddSubtitle(doc, _settings.Acceptance.SubTitle);

            AddProductTable(doc, acceptance.Products.GetProducts(), _localizer[VeloTexts.Price], includeDonationHint: true);

            AddSignature(doc, _settings.Acceptance.SignatureText, acceptance);

            
            if (_settings.Acceptance.StatusLinkFormat != null)
            {
                float length = _settings.QrCodeLengthMillimeters.ToUnit();
                string statusLink = string.Format(CultureInfo.InvariantCulture, _settings.Acceptance.StatusLinkFormat, acceptance.Seller.Token);
                string tokenText = _settings.Acceptance.GetTokenText(acceptance.Seller);

                BarcodeQRCode qrCode = new(statusLink);
                Image qrImage = new Image(qrCode.CreateFormXObject(pdfDoc))
                    .SetWidth(length)
                    .SetHeight(length);

                Paragraph barcodeAndLink = _pdf.GetRegularParagraph(tokenText)
                    .Add(qrImage)
                    .Add(statusLink)
                    .SetKeepTogether(true)
                    .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetBorderRadius(new BorderRadius(5))
                    .SetBorder(new SolidBorder(2))
                    .SetMarginTop(5.ToUnit())
                    .SetPadding(4.ToUnit())
                    .SetMaxWidth(length)
                    ;

                doc.Add(barcodeAndLink);
            }
        });
    }

    private byte[] CreateSale(TransactionEntity sale)
    {
        return CreateTransactionPdf((pdfDoc, doc) =>
        {
            AddBanner(doc);
            _pdf.AddBannerSubtitle(doc, _settings.BannerSubtitle, _settings.Website);

            _pdf.AddHeader(doc, null, GetLocationAndDateText(sale.Basar), null);

            _pdf.AddTitle(doc, string.Format(CultureInfo.CurrentCulture, _settings.Sale.TitleFormat, sale.Basar.Name, sale.Number));
            _pdf.AddSubtitle(doc, _settings.Sale.SubTitle);

            AddProductTable(doc, sale.Products.GetProducts(), _localizer[VeloTexts.Price], true, _settings.Sale.SellerInfoText);

            doc.Add(_pdf.GetSpacer(5));
            doc.Add(_pdf.GetRegularParagraph(_settings.Sale.HintText));
            //doc.Add(GetSpacer(2));
            doc.Add(_pdf.GetRegularParagraph(_settings.Sale.FooterText));
        });
    }

    private byte[] CreateSettlement(TransactionEntity settlement)
    {
        if (settlement.Seller == null)
        {
            throw new InvalidOperationException("Cannot create Settlement Document without Seller");
        }

        return CreateTransactionPdf((pdfDoc, doc) =>
        {
            IEnumerable<ProductEntity> products = settlement.Products.GetProducts();
            AddBanner(doc);
            _pdf.AddBannerSubtitle(doc, _settings.BannerSubtitle, _settings.Website);

            _pdf.AddHeader(doc, GetBigAddressText(settlement.Seller), GetLocationAndDateText(settlement.Basar), _localizer[VeloTexts.SellerIdShort, settlement.Seller.Id]);

            _pdf.AddTitle(doc, string.Format(CultureInfo.CurrentCulture, _settings.Settlement.TitleFormat, settlement.Basar.Name, settlement.Number));

            if (products.Any(p => p.ShouldBePayedOut()))
            {
                AddCommissionSummary(doc, settlement.GetPayoutTotal(), settlement.GetPayoutCommissionTotal(), settlement.GetPayoutTotalWithoutCommission(), settlement.Basar.ProductCommission);
                doc.Add(_pdf.GetSpacer(10));
            }

            IEnumerable<ProductEntity> payoutProducts = products.GetProductsToPayout();
            if (payoutProducts.Any())
            {
                _pdf.AddSubtitle(doc, _settings.Settlement.SoldTitle);
                AddProductTable(doc, payoutProducts, _localizer[VeloTexts.SellingPrice]);
            }

            IEnumerable<ProductEntity> pickupProducts = products.GetProductsToPickup();
            if (pickupProducts.Any())
            {
                _pdf.AddSubtitle(doc, _settings.Settlement.NotSoldTitle);
                AddProductTable(doc, pickupProducts, _localizer[VeloTexts.Price]);
            }

            doc.Add(_pdf.GetSpacer(5));
            doc.Add(_pdf.GetRegularParagraph(_settings.Settlement.ConfirmationText));

            AddSignature(doc, _settings.Settlement.SignatureText, settlement);

            if (settlement.NeedsBankingQrCodeOnDocument)
            {
                decimal amount = settlement.GetPayoutTotalWithoutCommission();
                string text = string.Format(CultureInfo.InvariantCulture, _settings.Settlement.BankTransactionTextFormat, settlement.Basar.Name);
                string epQrCode = GetEpQrCode(settlement.Seller.EffectiveBankAccountHolder, settlement.Seller.IBAN!, amount, text);

                float length = _settings.QrCodeLengthMillimeters.ToUnit();
                BarcodeQRCode qrCode = new(epQrCode);
                Image qrImage = new Image(qrCode.CreateFormXObject(pdfDoc))
                    .SetWidth(length)
                    .SetHeight(length);

                Paragraph barcode = new Paragraph()
                    .Add(qrImage)
                    .Add(_pdf.GetRegularParagraph(settlement.Seller.EffectiveBankAccountHolder))
                    .Add(Environment.NewLine)
                    .Add(_pdf.GetRegularParagraph(settlement.Seller.IBAN.ToPrettyIBAN()))
                    .SetKeepTogether(true)
                    .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetBorderRadius(new BorderRadius(5))
                    .SetBorder(new SolidBorder(2))
                    .SetMarginTop(5.ToUnit())
                    .SetPadding(4.ToUnit())
                    .SetMaxWidth(length)
                    ;

                doc.Add(barcode);
            }
        });
    }

    private void AddBanner(Document doc)
    {
        if (string.IsNullOrEmpty(_settings.BannerFilePath))
        {
            return;
        }
        if (!File.Exists(_settings.BannerFilePath))
        {
            throw new FileNotFoundException($"{nameof(PrintSettings.BannerFilePath)} '{_settings.BannerFilePath}' not found.");
        }
        ImageData bannerData = ImageDataFactory.Create(_settings.BannerFilePath);
        Image banner = new(bannerData);
        doc.Add(banner);
    }

    private byte[] CreateTransactionPdf(Action<PdfDocument, Document> decorate)
    {
        return _pdf.CreatePdf((pdfDoc, doc) =>
        {
            pdfDoc.SetDefaultPageSize(pageSize: PageSize.A4);
            doc.SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN));
            doc.SetMargins(_settings.PageMargins.Top, _settings.PageMargins.Right, _settings.PageMargins.Bottom + PageFooterHandler.Height, _settings.PageMargins.Left);
            pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, new PageFooterHandler(_settings.PageMargins, doc, _localizer));

            decorate(pdfDoc, doc);
        });
    }

    private void AddProductTable(Document doc, IEnumerable<ProductEntity> products, string priceColumnTitle, bool printSellerInfo = false, string? sellerInfoText = null, bool includeDonationHint = false)
    {
        ProductEntity[] productsArray = products.ToArray();

        Table productsTable = new Table(4)
            .UseAllAvailableWidth()
            .SetFontSize(_pdf.RegularFontSize);

        Cell idHeaderCell = new Cell()
            .SetBorderTop(null)
            .SetBorderLeft(null)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetVerticalAlignment(VerticalAlignment.MIDDLE)
            .Add(_pdf.GetBoldParagraph(_localizer[VeloTexts.Id]));

        Cell productInfoHeaderCell = new Cell()
            .SetBorderTop(null)
            .SetTextAlignment(TextAlignment.CENTER)
            .Add(_pdf.GetBoldParagraph(_localizer[VeloTexts.ProductDescription]));

        Cell sizeHeaderCell = new Cell()
            .SetBorderTop(null)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetVerticalAlignment(VerticalAlignment.MIDDLE)
            .Add(_pdf.GetBoldParagraph(_localizer[VeloTexts.Size]));

        Cell priceHeaderCell = new Cell()
            .SetBorderTop(null)
            .SetBorderRight(null)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetVerticalAlignment(VerticalAlignment.MIDDLE)
            .Add(_pdf.GetBoldParagraph(priceColumnTitle));

        productsTable.AddHeaderCell(idHeaderCell).AddHeaderCell(productInfoHeaderCell).AddHeaderCell(sizeHeaderCell).AddHeaderCell(priceHeaderCell);

        Cell sumFooterCell = new Cell()
            .SetBorderLeft(null)
            .SetBorderTop(new DoubleBorder(2))
            .SetBorderRight(null)
            .SetBorderBottom(null)
            .Add(_pdf.GetBoldParagraph($"{_localizer[VeloTexts.Sum]}:"));

        Cell productInfoFooterCell = new Cell(1, 2)
            .SetBorderLeft(null)
            .SetBorderTop(new DoubleBorder(2))
            .SetBorderRight(null)
            .SetBorderBottom(null)
            .Add(_pdf.GetBoldParagraph(_localizer[VeloTexts.ProductCounter, productsArray.Length]));

        Paragraph priceFooter = _pdf.GetBoldParagraph(string.Format(CultureInfo.CurrentCulture, "{0:C}", products.SumPrice()));
        Cell priceFooterCell = new Cell()
            .SetSplitCharacters(new NoSplitCharacters())
            .SetBorderLeft(null)
            .SetBorderTop(new DoubleBorder(2))
            .SetBorderRight(null)
            .SetBorderBottom(null)
            .SetTextAlignment(TextAlignment.RIGHT)
            .Add(priceFooter);

        productsTable.AddFooterCell(sumFooterCell).AddFooterCell(productInfoFooterCell).AddFooterCell(priceFooterCell);

        foreach (ProductEntity product in productsArray)
        {
            Paragraph id = new($"{product.Id}");
            Cell idCell = new Cell()
                .SetBorderLeft(null)
                .SetBorderRight(null)
                .Add(id);

            Paragraph productInfo = new(GetInfoText(product, includeDonationHint));
            Cell productInfoCell = new Cell()
                .SetBorderLeft(null)
                .SetBorderRight(null)
                .Add(productInfo);

            if (printSellerInfo)
            {
                SellerEntity seller = product.Session.Seller;
                Text sellerInfo = _pdf.GetSmallText($"* {GetSmallAddressText(seller)}");
                productInfoCell.Add(new Paragraph(sellerInfo));
            }

            Paragraph size = new(product.TireSize ?? "");
            Cell sizeCell = new Cell()
                .SetBorderLeft(null)
                .SetBorderRight(null)
                .SetTextAlignment(TextAlignment.RIGHT)
                .Add(size);

            Paragraph price = new Paragraph(string.Format(CultureInfo.CurrentCulture, "{0:C}", product.Price))
                .SetKeepTogether(true);
            Cell priceCell = new Cell()
                .SetSplitCharacters(new NoSplitCharacters())
                .SetBorderLeft(null)
                .SetBorderRight(null)
                .SetTextAlignment(TextAlignment.RIGHT);

            priceCell.Add(price);

            productsTable.AddCell(idCell).AddCell(productInfoCell).AddCell(sizeCell).AddCell(priceCell);
        }

        if (sellerInfoText != null)
        {
            Cell sellerInfoCell = new Cell(0, 2)
                .SetBorder(null)
                .Add(new Paragraph(_pdf.GetSmallText(sellerInfoText)));
            productsTable.AddCell(sellerInfoCell);
        }
        doc.Add(productsTable);
    }

    private string GetInfoText(ProductEntity product, bool includeDonationHint = false)
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

    private void AddCommissionSummary(Document doc, decimal total, decimal commisionAmount, decimal totalWithoutCommission, decimal commissionFactor)
    {
        Table productsTable = new Table(3)
            .UseAllAvailableWidth()
            .SetFontSize(_pdf.RegularFontSize);

        Cell column0HeaderCell = new Cell()
            .SetBorder(null)
            .Add(_pdf.GetBoldParagraph(_localizer[VeloTexts.IncomeFromSoldProducts]));

        Cell column1HeaderCell = new Cell()
            .SetBorder(null);

        Cell column2HeaderCell = new Cell()
            .SetBorder(null)
            .SetSplitCharacters(new NoSplitCharacters())
            .SetTextAlignment(TextAlignment.RIGHT)
            .Add(_pdf.GetBoldParagraph(string.Format(CultureInfo.CurrentCulture, "{0:C}", total)));

        productsTable.AddHeaderCell(column0HeaderCell).AddHeaderCell(column1HeaderCell).AddHeaderCell(column2HeaderCell);

        Cell commissionColumn0Cell = new Cell()
            .SetBorderLeft(null)
            .SetBorderRight(null)
            .Add(_pdf.GetRegularParagraph(_localizer[VeloTexts.SalesCommision, commissionFactor, total]));

        Cell commissionColumn1Cell = new Cell()
            .SetBorderLeft(null)
            .SetBorderRight(null)
            .SetSplitCharacters(new NoSplitCharacters())
            .SetTextAlignment(TextAlignment.RIGHT)
            .Add(_pdf.GetRegularParagraph(string.Format(CultureInfo.CurrentCulture, "{0:C}", commisionAmount)));

        Cell commissionColumn2Cell = new Cell()
            .SetBorderLeft(null)
            .SetBorderRight(null);

        productsTable.AddCell(commissionColumn0Cell).AddCell(commissionColumn1Cell).AddCell(commissionColumn2Cell);

        Cell costsColumn0Cell = new Cell()
            .SetBorderLeft(null)
            .SetBorderRight(null)
            .Add(_pdf.GetBoldParagraph($"{_localizer[VeloTexts.Cost]}:"));

        Cell costsColumn1Cell = new Cell()
            .SetBorderLeft(null)
            .SetBorderRight(null)
            .SetSplitCharacters(new NoSplitCharacters())
            .SetTextAlignment(TextAlignment.RIGHT)
            .Add(_pdf.GetRegularParagraph(string.Format(CultureInfo.CurrentCulture, "{0:C}", commisionAmount)));

        Cell costsColumn2Cell = new Cell()
            .SetBorderLeft(null)
            .SetBorderRight(null)
            .SetSplitCharacters(new NoSplitCharacters())
            .SetTextAlignment(TextAlignment.RIGHT)
            .Add(_pdf.GetBoldParagraph(string.Format(CultureInfo.CurrentCulture, "{0:C}", commisionAmount)));

        productsTable.AddCell(costsColumn0Cell).AddCell(costsColumn1Cell).AddCell(costsColumn2Cell);

        Cell totalColumn0Cell = new Cell()
            .SetBorderLeft(null)
            .SetBorderRight(null)
            .SetBorderBottom(null)
            .SetBorderTop(new DoubleBorder(2))
            .Add(_pdf.GetBigParagraph($"{_localizer[VeloTexts.TotalAmount]}:")
                .SetBold()
                .SetFontColor(_pdf.Orange));

        Cell totalColumn1Cell = new Cell()
            .SetBorderLeft(null)
            .SetBorderRight(null)
            .SetBorderBottom(null)
            .SetBorderTop(new DoubleBorder(2));

        Cell totalColumn2Cell = new Cell()
            .SetBorderLeft(null)
            .SetBorderRight(null)
            .SetBorderBottom(null)
            .SetBorderTop(new DoubleBorder(2))
            .SetSplitCharacters(new NoSplitCharacters())
            .SetTextAlignment(TextAlignment.RIGHT)
            .Add(_pdf.GetBigParagraph(string.Format(CultureInfo.CurrentCulture, "{0:C}", totalWithoutCommission))
                .SetBold()
                .SetFontColor(_pdf.Orange));

        productsTable.AddCell(totalColumn0Cell).AddCell(totalColumn1Cell).AddCell(totalColumn2Cell);

        doc.Add(productsTable);
    }

    private void AddSignature(Document doc, string text, TransactionEntity transaction)
    {
        Text signature = new Text($"{text} ______________________________")
            .SetFontSize(_pdf.RegularFontSize)
            .SetBold();
        LocalizedString locationAndDateString;
        if (string.IsNullOrEmpty(transaction.Basar.Location))
        {
            locationAndDateString = _localizer[VeloTexts.AtDateAndTime, transaction.TimeStamp];
        }
        else
        {
            locationAndDateString = _localizer[VeloTexts.AtLocationAndDateAndTime, transaction.Basar.Location, transaction.TimeStamp];
        }
        Text locationAndDate = new Text(locationAndDateString)
            .SetFontSize(_pdf.SmallFontSize);

        Paragraph p = new Paragraph()
            .Add(signature)
            .Add(Environment.NewLine)
            .Add(locationAndDate);
        doc.Add(p);
    }

    private static string GetLocationAndDateText(BasarEntity basar)
        => $"{basar.Location}, {basar.Date:d}";

    private static string? GetBigAddressText(SellerEntity? seller)
    {
        if (seller == null)
        {
            return null;
        }
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
