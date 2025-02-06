using System.IO;
using BraunauMobil.VeloBasar.Models.Documents;
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

namespace BraunauMobil.VeloBasar.Pdf;

public sealed class ItextTransactionDocumentGenerator(PdfGenerator pdf, IFormatProvider formatProvider)
    : ITransactionDocumentGenerator
{
    public async Task<byte[]> CreateAcceptanceAsync(AcceptanceDocumentModel model)
    {
        return await CreateTransactionPdfAsync((pdfDoc, doc) =>
        {
            pdf.AddHeader(doc, model.AddressText, model.LocationAndDateText, model.SellerIdText);

            pdf.AddTitle(doc, model.Title);
            pdf.AddSubtitle(doc, model.SubTitle);

            AddProductTable(doc, model.ProductsTable);

            if (model.AddTokenAndStatusLink)
            {
                float length = model.QrCodeLengthMillimeters.ToUnit();

                BarcodeQRCode qrCode = new(model.StatusLink);
                Image qrImage = new Image(qrCode.CreateFormXObject(pdfDoc))
                    .SetWidth(length)
                    .SetHeight(length)
                    .SetMargins(0, 0, 0, 0);

                Paragraph tokenTitleParagraph = pdf.GetMediumParagraph(model.TokenTitle)
                    .SetMargin(0);

                Paragraph tokenParagraph = pdf.GetRegularParagraph(model.SellerToken)
                    .SetMargin(0)
                    .SetFontColor(pdf.Red);

                Paragraph statusLinkParagraph = pdf.GetRegularParagraph(model.StatusLink);

                Div barcodeAndLink = new Div()
                    .Add(tokenTitleParagraph)
                    .Add(tokenParagraph)
                    .Add(qrImage)
                    .Add(statusLinkParagraph)
                    .SetKeepTogether(true)
                    .SetHorizontalAlignment(HorizontalAlignment.LEFT)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetBorderRadius(new BorderRadius(5))
                    .SetBorder(new SolidBorder(2))
                    .SetMarginTop(5.ToUnit())
                    .SetPadding(4.ToUnit())
                    .SetMaxWidth(length)
                    ;

                doc.Add(barcodeAndLink);
            }

            AddSignature(doc, model.SignatureLine, model.SignatureText);
        }, model);
    }

    public async Task<byte[]> CreateSaleAsync(SaleDocumentModel model)
    {
        return await CreateTransactionPdfAsync((pdfDoc, doc) =>
        {
            AddBanner(doc, model.BannerFilePath);
            pdf.AddBannerSubtitle(doc, model.BannerSubtitle, model.Website);

            pdf.AddHeader(doc, null, model.LocationAndDateText, null);

            pdf.AddTitle(doc, model.Title);
            pdf.AddSubtitle(doc, model.Subtitle);

            AddProductTable(doc, model.ProductsTable);

            doc.Add(pdf.GetSpacer(5));
            doc.Add(pdf.GetRegularParagraph(model.HintText));
            //doc.Add(GetSpacer(2));
            doc.Add(pdf.GetRegularParagraph(model.FooterText));
        }, model);
    }

    public async Task<byte[]> CreateSettlementAsync(SettlementDocumentModel model)
    {
        return await CreateTransactionPdfAsync((pdfDoc, doc) =>
        {
            AddBanner(doc, model.BannerFilePath);
            pdf.AddBannerSubtitle(doc, model.BannerSubtitle, model.Website);

            pdf.AddHeader(doc, model.AddressText, model.LocationAndDateText, model.SellerIdText);

            pdf.AddTitle(doc, model.Title);

            if (model.CommissionSummary is not null)
            {
                AddCommissionSummary(doc, model.CommissionSummary);
                doc.Add(pdf.GetSpacer(10));
            }

            if (model.PayoutProductsTable is not null)
            {
                pdf.AddSubtitle(doc, model.PayoutProductsTableTitle);
                AddProductTable(doc, model.PayoutProductsTable);
            }

            if (model.PickupProductsTable is not null)
            {
                pdf.AddSubtitle(doc, model.PickupProductsTableTitle);
                AddProductTable(doc, model.PickupProductsTable);
            }

            doc.Add(pdf.GetSpacer(5));
            doc.Add(pdf.GetRegularParagraph(model.ConfirmationText));

            if (model.AddBankingQrCode)
            {
                float length = model.QrCodeLengthMillimeters.ToUnit();
                BarcodeQRCode qrCode = new(model.BankingQrCodeContent);
                Image qrImage = new Image(qrCode.CreateFormXObject(pdfDoc))
                    .SetWidth(length)
                    .SetHeight(length);

                Paragraph barcode = new Paragraph()
                    .Add(qrImage)
                    .Add(pdf.GetRegularParagraph(model.BankAccountHolder))
                    .Add(Environment.NewLine)
                    .Add(pdf.GetRegularParagraph(model.IBAN))
                    .SetKeepTogether(true)
                    .SetHorizontalAlignment(HorizontalAlignment.LEFT)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetBorderRadius(new BorderRadius(5))
                    .SetBorder(new SolidBorder(2))
                    .SetMarginTop(5.ToUnit())
                    .SetPadding(4.ToUnit())
                    .SetMaxWidth(length)
                    ;

                doc.Add(barcode);
            }

            AddSignature(doc, model.SignatureLine, model.SignatureText);
        }, model);
    }

    private static void AddBanner(Document doc, string bannerFilePath)
    {
        if (string.IsNullOrEmpty(bannerFilePath))
        {
            return;
        }
        if (!File.Exists(bannerFilePath))
        {
            throw new FileNotFoundException($"{nameof(bannerFilePath)} '{bannerFilePath}' not found.");
        }
        ImageData bannerData = ImageDataFactory.Create(bannerFilePath);
        Image banner = new(bannerData);
        doc.Add(banner);
    }

    private async Task<byte[]> CreateTransactionPdfAsync(Action<PdfDocument, Document> decorate, ITransactionDocumentModel model)
    {
        return await Task.FromResult(pdf.CreatePdf((pdfDoc, doc) =>
        {
            pdfDoc.SetDefaultPageSize(pageSize: PageSize.A4);
            doc.SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN));
            doc.SetMargins(model.PageMargins.Top.ToUnit(), model.PageMargins.Right.ToUnit(), (model.PageMargins.Bottom + PageFooterHandler.Height).ToUnit(), model.PageMargins.Left.ToUnit());
            pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, new PageFooterHandler(model.PageMargins, doc, model.PageNumberFormat, model.PoweredBy, formatProvider));

            decorate(pdfDoc, doc);
        }));
    }

    private void AddProductTable(Document doc, ProductsTableDocumentModel model)
    {
        Table productsTable = new Table(4)
            .UseAllAvailableWidth()
            .SetFontSize(pdf.RegularFontSize);

        Cell idHeaderCell = new Cell()
            .SetBorderTop(null)
            .SetBorderLeft(null)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetVerticalAlignment(VerticalAlignment.MIDDLE)
            .Add(pdf.GetBoldParagraph(model.IdColumnTitle));

        Cell productInfoHeaderCell = new Cell()
            .SetBorderTop(null)
            .SetTextAlignment(TextAlignment.CENTER)
            .Add(pdf.GetBoldParagraph(model.ProductDescriptionColumnTitle));

        Cell sizeHeaderCell = new Cell()
            .SetBorderTop(null)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetVerticalAlignment(VerticalAlignment.MIDDLE)
            .Add(pdf.GetBoldParagraph(model.SizeColumnTitle));

        Cell priceHeaderCell = new Cell()
            .SetBorderTop(null)
            .SetBorderRight(null)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetVerticalAlignment(VerticalAlignment.MIDDLE)
            .Add(pdf.GetBoldParagraph(model.PriceColumnTitle));

        productsTable.AddHeaderCell(idHeaderCell).AddHeaderCell(productInfoHeaderCell).AddHeaderCell(sizeHeaderCell).AddHeaderCell(priceHeaderCell);

        Cell sumFooterCell = new Cell()
            .SetBorderLeft(null)
            .SetBorderTop(new DoubleBorder(2))
            .SetBorderRight(null)
            .SetBorderBottom(null)
            .Add(pdf.GetBoldParagraph(model.SumText));

        Cell productInfoFooterCell = new Cell(1, 2)
            .SetBorderLeft(null)
            .SetBorderTop(new DoubleBorder(2))
            .SetBorderRight(null)
            .SetBorderBottom(null)
            .Add(pdf.GetBoldParagraph(model.CountText));

        Paragraph priceFooter = pdf.GetBoldParagraph(model.PriceText);
        Cell priceFooterCell = new Cell()
            .SetSplitCharacters(new NoSplitCharacters())
            .SetBorderLeft(null)
            .SetBorderTop(new DoubleBorder(2))
            .SetBorderRight(null)
            .SetBorderBottom(null)
            .SetTextAlignment(TextAlignment.RIGHT)
            .Add(priceFooter);

        productsTable.AddFooterCell(sumFooterCell).AddFooterCell(productInfoFooterCell).AddFooterCell(priceFooterCell);

        foreach (ProductTableRowDocumentModel row in model.Rows)
        {
            Paragraph id = new(row.Id);
            Cell idCell = new Cell()
                .SetBorderLeft(null)
                .SetBorderRight(null)
                .Add(id);

            Paragraph productInfo = new(row.InfoText);
            Cell productInfoCell = new Cell()
                .SetBorderLeft(null)
                .SetBorderRight(null)
                .Add(productInfo);

            if (row.SellerInfo is not null)
            {
                Text sellerInfo = pdf.GetSmallText(row.SellerInfo);
                productInfoCell.Add(new Paragraph(sellerInfo));
            }

            Paragraph size = new(row.TireSize);
            Cell sizeCell = new Cell()
                .SetBorderLeft(null)
                .SetBorderRight(null)
                .SetTextAlignment(TextAlignment.RIGHT)
                .Add(size);

            Paragraph price = new Paragraph(row.Price)
                .SetKeepTogether(true);
            Cell priceCell = new Cell()
                .SetSplitCharacters(new NoSplitCharacters())
                .SetBorderLeft(null)
                .SetBorderRight(null)
                .SetTextAlignment(TextAlignment.RIGHT);

            priceCell.Add(price);

            productsTable.AddCell(idCell).AddCell(productInfoCell).AddCell(sizeCell).AddCell(priceCell);
        }

        if (model.SellerInfoText is not null)
        {
            Cell sellerInfoCell = new Cell(0, 2)
                .SetBorder(null)
                .Add(new Paragraph(pdf.GetSmallText(model.SellerInfoText)));
            productsTable.AddCell(sellerInfoCell);
        }
        doc.Add(productsTable);
    }

    private void AddCommissionSummary(Document doc, SettlementCommisionSummaryModel model)
    {
        Table commissionTable = new Table(3)
            .UseAllAvailableWidth()
            .SetFontSize(pdf.RegularFontSize);

        Cell column0HeaderCell = new Cell()
            .SetBorder(null)
            .Add(pdf.GetBoldParagraph(model.IncomeFromSoldProductsText));

        Cell column1HeaderCell = new Cell()
            .SetBorder(null);

        Cell column2HeaderCell = new Cell()
            .SetBorder(null)
            .SetSplitCharacters(new NoSplitCharacters())
            .SetTextAlignment(TextAlignment.RIGHT)
            .Add(pdf.GetBoldParagraph(model.PayoutAmountInclCommissionText));

        commissionTable.AddHeaderCell(column0HeaderCell).AddHeaderCell(column1HeaderCell).AddHeaderCell(column2HeaderCell);

        Cell commissionColumn0Cell = new Cell()
            .SetBorderLeft(null)
            .SetBorderRight(null)
            .Add(pdf.GetRegularParagraph(model.CommissionPartText));

        Cell commissionColumn1Cell = new Cell()
            .SetBorderLeft(null)
            .SetBorderRight(null)
            .SetSplitCharacters(new NoSplitCharacters())
            .SetTextAlignment(TextAlignment.RIGHT)
            .Add(pdf.GetRegularParagraph(model.PayoutCommissionAmountText));

        Cell commissionColumn2Cell = new Cell()
            .SetBorderLeft(null)
            .SetBorderRight(null);

        commissionTable.AddCell(commissionColumn0Cell).AddCell(commissionColumn1Cell).AddCell(commissionColumn2Cell);

        Cell costsColumn0Cell = new Cell()
            .SetBorderLeft(null)
            .SetBorderRight(null)
            .Add(pdf.GetBoldParagraph(model.CostsText));

        Cell costsColumn1Cell = new Cell()
            .SetBorderLeft(null)
            .SetBorderRight(null)
            .SetSplitCharacters(new NoSplitCharacters())
            .SetTextAlignment(TextAlignment.RIGHT)
            .Add(pdf.GetRegularParagraph(model.PayoutCommissionAmountText));

        Cell costsColumn2Cell = new Cell()
            .SetBorderLeft(null)
            .SetBorderRight(null)
            .SetSplitCharacters(new NoSplitCharacters())
            .SetTextAlignment(TextAlignment.RIGHT)
            .Add(pdf.GetBoldParagraph(model.PayoutCommissionAmountText));

        commissionTable.AddCell(costsColumn0Cell).AddCell(costsColumn1Cell).AddCell(costsColumn2Cell);

        Cell totalColumn0Cell = new Cell()
            .SetBorderLeft(null)
            .SetBorderRight(null)
            .SetBorderBottom(null)
            .SetBorderTop(new DoubleBorder(2))
            .Add(pdf.GetBigParagraph(model.TotalAmountText)
                .SetBold()
                .SetFontColor(pdf.Orange));

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
            .Add(pdf.GetBigParagraph(model.PayoutAmountText))
                .SetBold()
                .SetFontColor(pdf.Orange);

        commissionTable.AddCell(totalColumn0Cell).AddCell(totalColumn1Cell).AddCell(totalColumn2Cell);

        doc.Add(commissionTable);
    }
    private void AddSignature(Document doc, string line, string text)
    {
        Text signature = new Text(line)
            .SetFontSize(pdf.RegularFontSize)
            .SetBold();
        Text locationAndDate = new Text(text)
            .SetFontSize(pdf.SmallFontSize);

        Paragraph p = new Paragraph()
            .Add(signature)
            .Add(Environment.NewLine)
            .Add(locationAndDate)
            .SetKeepTogether(true)
            .SetMarginTop(25)
            .SetTextAlignment(TextAlignment.RIGHT)
            .SetHorizontalAlignment(HorizontalAlignment.RIGHT);
        doc.Add(p);
    }
}
