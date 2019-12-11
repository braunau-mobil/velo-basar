using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Resources;
using iText.Barcodes;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Printing
{
    public class PdfPrintService : IPrintService
    {
        private const int _bigFontSize = 14;
        private const int _mediumFontSize = 12;
        private const int _smallFontSize = 8;
        private const int _regularFontSize = 10;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public PdfPrintService(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        public byte[] Combine(IEnumerable<byte[]> pdfs)
        {
            byte[] bytes;
            using (var combinedStream = new MemoryStream())
            {
                using (var combinedPdfWriter = new PdfWriter(combinedStream))
                using (var combinedPdfDoc = new PdfDocument(combinedPdfWriter))
                {
                    foreach (var pdf in pdfs)
                    {
                        using (var pfdStream = new MemoryStream(pdf))
                        using (var pdfReader = new PdfReader(pfdStream))
                        using (var pdfDoc = new PdfDocument(pdfReader))
                        {
                            pdfDoc.CopyPagesTo(1, pdfDoc.GetNumberOfPages(), combinedPdfDoc);
                        }
                    }
                }

                bytes = combinedStream.GetBuffer();
            }
            return bytes;
        }

        public byte[] CreateAcceptance(ProductsTransaction acceptance, PrintSettings settings)
        {
            Contract.Requires(acceptance != null);
            Contract.Requires(settings != null);

            return CreateTransactionPdf(settings, (pdfDoc, doc) =>
            {
                AddHeader(doc, acceptance.Seller.GetAddressText(), acceptance.Basar.GetLocationAndDateText(), acceptance.Seller.GetIdText(_localizer));

                AddTitle(doc, string.Format(CultureInfo.CurrentCulture, settings.Acceptance.TitleFormat, acceptance.Basar.Name), settings.Acceptance.SubTitle);

                AddProductTable(doc, acceptance, false);

                var signature = GetSignatureText(acceptance, settings.Acceptance);
                signature.SetFontSize(_regularFontSize);
                signature.SetPaddingTop(20);
                doc.Add(signature);

                var token = new Paragraph(settings.Acceptance.GetTokenText(acceptance.Seller));
                token.SetFontSize(_regularFontSize);
                token.SetMarginTop(10);
                doc.Add(token);
            });
        }

        public byte[] CreateLabel(Basar basar, Product product)
        {
            return CreatePdf((pdfDoc, doc) =>
            {
                doc.SetMargins(0, 0, 0, 0);
                doc.SetFontSize(7);

                var page = pdfDoc.AddNewPage(new PageSize(50f.ToUnit(), 79f.ToUnit()));

                doc.Add(new Paragraph($"Braunau mobil - {basar.Name}"));
                doc.Add(new Paragraph(product.Brand.Name));
                doc.Add(new Paragraph(product.Type.Name));
                doc.Add(new Paragraph(product.Description));
                doc.Add(new Paragraph($"Reifengröße: {product.TireSize}"));
                doc.Add(new Paragraph(product.Price.ToString()));

                var barcode = new BarcodeEAN(pdfDoc);
                barcode.SetCode($"{product.Id:0000000000000}");
                doc.Add(new Image(barcode.CreateFormXObject(pdfDoc)));
            });
        }

        public byte[] CreateSale(ProductsTransaction sale, PrintSettings settings)
        {
            return CreateTransactionPdf(settings, (pdfDoc, doc) =>
            {
                var bannerData = ImageDataFactory.Create(settings.Banner.Bytes);
                var banner = new Image(bannerData);
                doc.Add(new Image(bannerData));

                AddBannerSubtitle(doc, settings.BannerSubtitle, settings.Website);

                AddHeader(doc, null, sale.Basar.GetLocationAndDateText(), null);

                AddTitle(doc, string.Format(CultureInfo.CurrentCulture, settings.Sale.TitleFormat, sale.Basar.Name), settings.Sale.SubTitle);

                AddProductTable(doc, sale, true);

                var hintText = GetRegularTet(settings.Sale.HintText)
                    .SetMarginTop(20)
                    .SetMarginBottom(20);
                doc.Add(hintText);

                doc.Add(GetRegularTet(settings.Sale.FooterText));
            });
        }

        public byte[] CreateSettlement(ProductsTransaction settlement)
        {
            return CreatePdf((pdfDoc, doc) =>
            {
                var page = pdfDoc.AddNewPage(PageSize.A5);

                doc.Add(new Paragraph($"Braunau mobil - {settlement.Basar.Name}"));
                doc.Add(new Paragraph($"Abrechnung #{settlement.Number}"));
            });
        }

        private byte[] CreatePdf(Action<PdfDocument, Document> decorate)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                using (var pdfWriter = new PdfWriter(memoryStream))
                using (var pdfDoc = new PdfDocument(pdfWriter))
                using (var doc = new Document(pdfDoc))
                {
                    decorate(pdfDoc, doc);
                }

                bytes = memoryStream.GetBuffer();
            }
            return bytes;
        }
        private byte[] CreateTransactionPdf(PrintSettings settings, Action<PdfDocument, Document> decorate)
        {
            return CreatePdf((pdfDoc, doc) =>
            {
                doc.SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN));
                doc.SetMargins(settings.PageMargins.Top, settings.PageMargins.Right, settings.PageMargins.Bottom, settings.PageMargins.Left);
                pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, new PageFooterHandler(doc, _localizer));

                decorate(pdfDoc, doc);
            });
        }

        private void AddBannerSubtitle(Document doc, string bannerSubtitleText, string websiteText)
        {
            var bannerSubtitle = GetRegularTet(bannerSubtitleText)
                .SetMargin(0)
                .SetTextAlignment(TextAlignment.CENTER);
            doc.Add(bannerSubtitle);

            var website = GetRegularTet(websiteText)
                .SetMarginTop(0)
                .SetMarginBottom(10)
                .SetFontColor(ColorConstants.GREEN)
                .SetTextAlignment(TextAlignment.CENTER);
            doc.Add(website);
        }
        private void AddHeader(Document doc, string addressText, string locationAndDateText, string sellerIdText)
        {
            var headerTable = new Table(2);
            headerTable.UseAllAvailableWidth();
            
            var addressCell = new Cell(2, 1)
                .SetBorder(null);
            if (addressText != null)
            {
                addressCell.Add(GetRegularTet(addressText));
            }

            var locationAndDateCell = new Cell()
                .SetBorder(null)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetVerticalAlignment(VerticalAlignment.TOP);
            if(locationAndDateText != null)
            {
                locationAndDateCell.Add(GetRegularTet(locationAndDateText));
            }

            var sellerIdCell = new Cell()
                .SetBorder(null)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetVerticalAlignment(VerticalAlignment.BOTTOM);
            if (sellerIdText != null)
            {
                sellerIdCell.Add(GetRegularTet(sellerIdText));
            }
            
            headerTable.AddCell(addressCell).AddCell(locationAndDateCell).AddCell(sellerIdCell);
            doc.Add(headerTable);
        }
        private void AddProductTable(Document doc, ProductsTransaction transaction, bool displaySellerInfo)
        {
            var productsTable = new Table(4)
                .UseAllAvailableWidth()
                .SetFontSize(_regularFontSize);

            var idHeaderCell = new Cell()
                .SetBorderTop(null)
                .SetBorderLeft(null)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(GetBoldText(_localizer["Id"]));

            var productInfoHeaderCell = new Cell()
                .SetBorderTop(null)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(GetBoldText(_localizer["Hersteller - Artikel\r\nZusatzinformationen"]));

            var sizeHeaderCell = new Cell()
                .SetBorderTop(null)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(GetBoldText(_localizer["Größe"]));

            var priceHeaderCell = new Cell()
                .SetBorderTop(null)
                .SetBorderRight(null)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(GetBoldText(_localizer["Preis"]));

            productsTable.AddHeaderCell(idHeaderCell).AddHeaderCell(productInfoHeaderCell).AddHeaderCell(sizeHeaderCell).AddHeaderCell(priceHeaderCell);

            var sumFooterCell = new Cell()
                .SetBorderLeft(null)
                .SetBorderTop(new DoubleBorder(2))
                .SetBorderRight(null)
                .SetBorderBottom(null)
                .Add(GetBoldText(_localizer["Summe:"]));

            var productInfoFooterCell = new Cell(1, 2)
                .SetBorderLeft(null)
                .SetBorderTop(new DoubleBorder(2))
                .SetBorderRight(null)
                .SetBorderBottom(null)
                .Add(GetBoldText(_localizer["{0} Artikel", transaction.Products.Count]));

            var priceFooter = GetBoldText(transaction.GetSumText())
                .SetPaddingLeft(20);
            var priceFooterCell = new Cell()
                .SetSplitCharacters(new NoSplitCharacters())
                .SetBorderLeft(null)
                .SetBorderTop(new DoubleBorder(2))
                .SetBorderRight(null)
                .SetBorderBottom(null)
                .Add(priceFooter);

            productsTable.AddFooterCell(sumFooterCell).AddFooterCell(productInfoFooterCell).AddFooterCell(priceFooterCell);

            foreach (var product in transaction.Products.Select(x => x.Product))
            {
                var id = new Paragraph($"{product.Id}");
                var idCell = new Cell()
                    .SetBorderLeft(null)
                    .SetBorderRight(null)
                    .Add(id);

                var productInfo = new Paragraph(product.GetInfoText(_localizer));
                var productInfoCell = new Cell()
                    .SetBorderLeft(null)
                    .SetBorderRight(null)
                    .Add(productInfo);

                if (displaySellerInfo)
                {
                    var sellerInfo = GetSmallText("@TODO");
                    productInfoCell.Add(sellerInfo);
                }

                var size = new Paragraph(product.TireSize);
                var sizeCell = new Cell()
                    .SetBorderLeft(null)
                    .SetBorderRight(null)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .Add(size);

                var price = new Paragraph(product.GetPriceText(transaction))
                    .SetKeepTogether(true);
                var priceCell = new Cell()
                    .SetSplitCharacters(new NoSplitCharacters())
                    .SetBorderLeft(null)
                    .SetBorderRight(null)
                    .SetTextAlignment(TextAlignment.RIGHT);

                priceCell.Add(price);

                productsTable.AddCell(idCell).AddCell(productInfoCell).AddCell(sizeCell).AddCell(priceCell);
            }
            doc.Add(productsTable);
        }
        private void AddTitle(Document doc, string titleText, string subtitleText)
        {
            var mainTitle = GetBigText(titleText)
                .SetBold()
                .SetMarginBottom(20);
            doc.Add(mainTitle);

            var subTitle = GetMediumText(subtitleText)
                .SetBold()
                .SetMarginBottom(10);
            doc.Add(subTitle);
        }
        private Paragraph GetSignatureText(ProductsTransaction transaction, AcceptancePrintSettings settings)
        {
            Contract.Requires(transaction != null);
            Contract.Requires(settings != null);

            var signature = new Text(settings.GetSignatureText(transaction, _localizer));
            signature.SetBold();
            var locationAndDate = new Text(_localizer["{0} am {1} Uhr", transaction.Basar.Location, transaction.TimeStamp]);

            var p = new Paragraph();
            p.Add(signature);
            p.Add(locationAndDate);

            return p;
        }

        private static Paragraph GetBigText(string text)
        {
            return new Paragraph(text)
                .SetFontSize(_bigFontSize);
        }
        private static Paragraph GetBoldText(string text)
        {
            return new Paragraph(text)
                .SetBold();
        }
        private static Paragraph GetMediumText(string text)
        {
            return new Paragraph(text)
                .SetFontSize(_mediumFontSize);
        }
        private static Paragraph GetRegularTet(string text)
        {
            return new Paragraph(text)
                .SetFontSize(_regularFontSize);
        }
        private static Paragraph GetSmallText(string text)
        {
            return new Paragraph(text)
                .SetFontSize(_smallFontSize);
        }
    }
}
