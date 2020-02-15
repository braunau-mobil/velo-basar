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
using iColor = iText.Kernel.Colors.Color;

namespace BraunauMobil.VeloBasar.Printing
{
    public class PdfPrintService : IPrintService
    {
        private const int _bigFontSize = 14;
        private const int _mediumFontSize = 12;
        private const int _smallFontSize = 8;
        private const int _regularFontSize = 10;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly iColor _orange;
        private readonly iColor _green;

        public PdfPrintService(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
            _orange = iColor.MakeColor(ColorConstants.GREEN.GetColorSpace(), new float[] { 1f, 0.5f, 0f });
            _green = iColor.MakeColor(ColorConstants.GREEN.GetColorSpace(), new float[] { 0f, 0.5f, 0f });
        }

        public byte[] Combine(IEnumerable<byte[]> pdfs)
        {
            Contract.Requires(pdfs != null);

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
        public byte[] CreateTransaction(ProductsTransaction transaction, PrintSettings settings)
        {
            Contract.Requires(transaction != null);

            if (transaction.Type == TransactionType.Acceptance)
            {
                return CreateAcceptance(transaction, settings);
            }
            else if (transaction.Type == TransactionType.Sale)
            {
                return CreateSale(transaction, settings);
            }
            else if (transaction.Type == TransactionType.Settlement)
            {
                return CreateSettlement(transaction, settings);
            }

            throw new InvalidOperationException($"Cannot generate transaction document for: {transaction.Type}");
        }        
        public byte[] CreateLabel(Product product, PrintSettings settings)
        {
            return CreatePdf((pdfDoc, doc) =>
            {
                pdfDoc.AddNewPage(new PageSize(45f.ToUnit(), 79f.ToUnit()));
                //doc.SetTextRenderingMode(3);

                doc.SetMargins(5, 5, 5, 5);
                doc.SetFontSize(10);
                doc.SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN));

                doc.Add(new Paragraph(
                    GetSmallText(string.Format(CultureInfo.CurrentCulture, settings.Label.TitleFormat, product.Basar.Name)))
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetBorderBottom(new SolidBorder(2)));

                var info = new Paragraph()
                    .Add(GetSmallText($"{product.Brand.Name} - {product.Type.Name}").SetBold())
                    .Add(Environment.NewLine)
                    .Add(GetSmallText(product.Description));

                if (product.Color != null)
                {
                    info.Add(Environment.NewLine)
                        .Add(GetSmallText(product.Color));
                }

                if (product.FrameNumber != null)
                {
                    info.Add(Environment.NewLine)
                        .Add(GetSmallText(_localizer["Rahmennummer: {0}", product.FrameNumber]));
                }

                if (product.TireSize != null)
                {
                    info.Add(Environment.NewLine)
                        .Add(GetSmallText(_localizer["Reifengröße: {0}", product.TireSize]));

                }

                info.SetMargin(2);
                doc.Add(info);

                
                var barcodeAndPrice = new Paragraph()
                    .SetTextAlignment(TextAlignment.CENTER);
                var barcode = new Barcode128(pdfDoc);
                barcode.SetCode($"{product.Id}");
                var price = GetBigText(string.Format(CultureInfo.CurrentCulture, "{0:C}", product.Price))
                    .SetBold()
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetBorderTop(new SolidBorder(2))
                    .SetWidth(40f.ToUnit());

                barcodeAndPrice
                    .Add(new Image(barcode.CreateFormXObject(pdfDoc)))
                    .Add(new Text(Environment.NewLine))
                    .Add(price)
                    .SetFixedPosition(5f, 5f, 40f.ToUnit());

                doc.Add(barcodeAndPrice);
            });
        }

        private byte[] CreateAcceptance(ProductsTransaction acceptance, PrintSettings settings)
        {
            return CreateTransactionPdf(settings, (pdfDoc, doc) =>
            {
                AddHeader(doc, acceptance.Seller.GetBigAddressText(), acceptance.Basar.GetLocationAndDateText(), acceptance.Seller.GetIdText(_localizer));

                AddTitle(doc, string.Format(CultureInfo.CurrentCulture, settings.Acceptance.TitleFormat, acceptance.Basar.Name, acceptance.Number));
                AddSubtitle(doc, settings.Acceptance.SubTitle);

                AddProductTable(doc, acceptance.Products.GetProducts(), _localizer["Preis"]);

                AddSignature(doc, settings.Acceptance.SignatureText, acceptance);

                doc.Add(GetSpacer(10));
                doc.Add(GetRegularText(settings.Acceptance.GetTokenText(acceptance.Seller)));
            });
        }
        private byte[] CreateSale(ProductsTransaction sale, PrintSettings settings)
        {
            return CreateTransactionPdf(settings, (pdfDoc, doc) =>
            {
                if (settings.Banner.Bytes != null)
                {
                    var bannerData = ImageDataFactory.Create(settings.Banner.Bytes);
                    var banner = new Image(bannerData);
                    doc.Add(new Image(bannerData));
                }
                AddBannerSubtitle(doc, settings.BannerSubtitle, settings.Website);

                AddHeader(doc, null, sale.Basar.GetLocationAndDateText(), null);

                AddTitle(doc, string.Format(CultureInfo.CurrentCulture, settings.Sale.TitleFormat, sale.Basar.Name, sale.Number));
                AddSubtitle(doc, settings.Sale.SubTitle);

                AddProductTable(doc, sale.Products.GetProducts(), _localizer["Preis"], true, settings.Sale.SellerInfoText);

                doc.Add(GetSpacer(20));
                doc.Add(GetRegularText(settings.Sale.HintText));
                doc.Add(GetSpacer(20));
                doc.Add(GetRegularText(settings.Sale.FooterText));
            });
        }
        private byte[] CreateSettlement(ProductsTransaction settlement, PrintSettings settings)
        {
            return CreateTransactionPdf(settings, (pdfDoc, doc) =>
            {
                var products = settlement.Products.GetProducts();

                if (settings.Banner.Bytes != null)
                {
                    var bannerData = ImageDataFactory.Create(settings.Banner.Bytes);
                    var banner = new Image(bannerData);
                    doc.Add(new Image(bannerData));
                }
                AddBannerSubtitle(doc, settings.BannerSubtitle, settings.Website);

                AddHeader(doc, settlement.Seller.GetBigAddressText(), settlement.Basar.GetLocationAndDateText(), settlement.Seller.GetIdText(_localizer));

                AddTitle(doc, string.Format(CultureInfo.CurrentCulture, settings.Settlement.TitleFormat, settlement.Basar.Name, settlement.Number));

                if (products.Any(p => p.StorageState == StorageState.Sold))
                {
                    AddCommissionSummary(doc, settlement.GetSoldProductsSum(), settlement.GetSoldCommissionSum(), settlement.GetSoldTotal(), settlement.Basar.ProductCommission);
                    doc.Add(GetSpacer(20));
                }

                AddSubtitle(doc, settings.Settlement.SoldTitle);
                AddProductTable(doc, products.GetProductsToPayout(), _localizer["Verkaufspreis"]);

                AddSubtitle(doc, settings.Settlement.NotSoldTitle);
                AddProductTable(doc, products.GetProductsToPickup(), _localizer["Preis"]);

                doc.Add(GetSpacer(20));
                doc.Add(GetRegularText(settings.Settlement.ConfirmationText));
                doc.Add(GetSpacer(20));
                AddSignature(doc, settings.Settlement.SignatureText, settlement);
            });
        }

        private static byte[] CreatePdf(Action<PdfDocument, Document> decorate)
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
                pdfDoc.SetDefaultPageSize(pageSize: PageSize.A5);
                doc.SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN));
                doc.SetMargins(settings.PageMargins.Top, settings.PageMargins.Right, settings.PageMargins.Bottom, settings.PageMargins.Left);
                pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, new PageFooterHandler(doc, _localizer));

                decorate(pdfDoc, doc);
            });
        }

        private void AddBannerSubtitle(Document doc, string bannerSubtitleText, string websiteText)
        {
            var bannerSubtitle = GetRegularText(bannerSubtitleText)
                .SetMargin(0)
                .SetTextAlignment(TextAlignment.CENTER);
            doc.Add(bannerSubtitle);

            var website = GetRegularText(websiteText)
                .SetMarginTop(0)
                .SetMarginBottom(10)
                .SetFontColor(_green)
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
                addressCell.Add(GetRegularText(addressText));
            }

            var locationAndDateCell = new Cell()
                .SetBorder(null)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetVerticalAlignment(VerticalAlignment.TOP);
            if(locationAndDateText != null)
            {
                locationAndDateCell.Add(GetRegularText(locationAndDateText));
            }

            var sellerIdCell = new Cell()
                .SetBorder(null)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetVerticalAlignment(VerticalAlignment.BOTTOM);
            if (sellerIdText != null)
            {
                sellerIdCell.Add(GetRegularText(sellerIdText));
            }
            
            headerTable.AddCell(addressCell).AddCell(locationAndDateCell).AddCell(sellerIdCell);
            doc.Add(headerTable);
        }
        private void AddProductTable(Document doc, IEnumerable<Product> products, string priceColumnTitle, bool printSellerInfo = false, string sellerInfoText = null)
        {
            var productsTable = new Table(4)
                .UseAllAvailableWidth()
                .SetFontSize(_regularFontSize);

            var idHeaderCell = new Cell()
                .SetBorderTop(null)
                .SetBorderLeft(null)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                .Add(GetBoldText(_localizer["Id"]));

            var productInfoHeaderCell = new Cell()
                .SetBorderTop(null)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(GetBoldText(_localizer["Hersteller - Artikel\r\nZusatzinformationen"]));

            var sizeHeaderCell = new Cell()
                .SetBorderTop(null)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                .Add(GetBoldText(_localizer["Größe"]));

            var priceHeaderCell = new Cell()
                .SetBorderTop(null)
                .SetBorderRight(null)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                .Add(GetBoldText(priceColumnTitle));

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
                .Add(GetBoldText(_localizer["{0} Artikel", products.Count()]));

            var priceFooter = GetBoldText(string.Format(CultureInfo.CurrentCulture, "{0:C}", products.SumPrice()));
            var priceFooterCell = new Cell()
                .SetSplitCharacters(new NoSplitCharacters())
                .SetBorderLeft(null)
                .SetBorderTop(new DoubleBorder(2))
                .SetBorderRight(null)
                .SetBorderBottom(null)
                .SetTextAlignment(TextAlignment.RIGHT)
                .Add(priceFooter);

            productsTable.AddFooterCell(sumFooterCell).AddFooterCell(productInfoFooterCell).AddFooterCell(priceFooterCell);

            foreach (var product in products)
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

                if (printSellerInfo)
                {
                    var seller = product.Seller;
                    var sellerInfo = GetSmallText($"* {seller.GetSmallAddressText()}");
                    productInfoCell.Add(new Paragraph(sellerInfo));
                }

                var size = new Paragraph(product.TireSize ?? "");
                var sizeCell = new Cell()
                    .SetBorderLeft(null)
                    .SetBorderRight(null)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .Add(size);

                var price = new Paragraph(string.Format(CultureInfo.CurrentCulture, "{0:C}", product.Price))
                    .SetKeepTogether(true);
                var priceCell = new Cell()
                    .SetSplitCharacters(new NoSplitCharacters())
                    .SetBorderLeft(null)
                    .SetBorderRight(null)
                    .SetTextAlignment(TextAlignment.RIGHT);

                priceCell.Add(price);

                productsTable.AddCell(idCell).AddCell(productInfoCell).AddCell(sizeCell).AddCell(priceCell);
            }

            if (sellerInfoText != null)
            {
                var sellerInfoCell = new Cell(0, 2)
                    .SetBorder(null)
                    .Add(new Paragraph(GetSmallText(sellerInfoText)));
                productsTable.AddCell(sellerInfoCell);
            }
            doc.Add(productsTable);
        }
        private void AddCommissionSummary(Document doc, decimal sum, decimal commisionAmount, decimal total, decimal commissionFactor)
        {
            var productsTable = new Table(3)
                .UseAllAvailableWidth()
                .SetFontSize(_regularFontSize);

            var column0HeaderCell = new Cell()
                .SetBorder(null)
                .Add(GetBoldText(_localizer["Einnahmen aus verkauften Artikeln:"]));

            var column1HeaderCell = new Cell()
                .SetBorder(null);

            var column2HeaderCell = new Cell()
                .SetBorder(null)
                .SetSplitCharacters(new NoSplitCharacters())
                .SetTextAlignment(TextAlignment.RIGHT)
                .Add(GetBoldText(string.Format(CultureInfo.CurrentCulture, "{0:C}", sum)));

            productsTable.AddHeaderCell(column0HeaderCell).AddHeaderCell(column1HeaderCell).AddHeaderCell(column2HeaderCell);

            var commissionColumn0Cell = new Cell()
                .SetBorderLeft(null)
                .SetBorderRight(null)
                .Add(GetRegularText(_localizer["Verkaufsprovision ({0:P2} von {1:C}):", commissionFactor, sum]));

            var commissionColumn1Cell = new Cell()
                .SetBorderLeft(null)
                .SetBorderRight(null)
                .SetSplitCharacters(new NoSplitCharacters())
                .SetTextAlignment(TextAlignment.RIGHT)
                .Add(GetRegularText(string.Format(CultureInfo.CurrentCulture, "{0:C}", commisionAmount)));

            var commissionColumn2Cell = new Cell()
                .SetBorderLeft(null)
                .SetBorderRight(null);
            
            productsTable.AddCell(commissionColumn0Cell).AddCell(commissionColumn1Cell).AddCell(commissionColumn2Cell);

            var costsColumn0Cell = new Cell()
                .SetBorderLeft(null)
                .SetBorderRight(null)
                .Add(GetBoldText(_localizer["Kosten:"]));

            var costsColumn1Cell = new Cell()
                .SetBorderLeft(null)
                .SetBorderRight(null)
                .SetSplitCharacters(new NoSplitCharacters())
                .SetTextAlignment(TextAlignment.RIGHT)
                .Add(GetRegularText(string.Format(CultureInfo.CurrentCulture, "{0:C}", commisionAmount)));

            var costsColumn2Cell = new Cell()
                .SetBorderLeft(null)
                .SetBorderRight(null)
                .SetSplitCharacters(new NoSplitCharacters())
                .SetTextAlignment(TextAlignment.RIGHT)
                .Add(GetBoldText(string.Format(CultureInfo.CurrentCulture, "{0:C}", commisionAmount)));

            productsTable.AddCell(costsColumn0Cell).AddCell(costsColumn1Cell).AddCell(costsColumn2Cell);

            var totalColumn0Cell = new Cell()
                .SetBorderLeft(null)
                .SetBorderRight(null)
                .SetBorderBottom(null)
                .SetBorderTop(new DoubleBorder(2))
                .Add(GetBigText(_localizer["Gesamtbetrag:"])
                    .SetBold()
                    .SetFontColor(_orange));

            var totalColumn1Cell = new Cell()
                .SetBorderLeft(null)
                .SetBorderRight(null)
                .SetBorderBottom(null)
                .SetBorderTop(new DoubleBorder(2));

            var totalColumn2Cell = new Cell()
                .SetBorderLeft(null)
                .SetBorderRight(null)
                .SetBorderBottom(null)
                .SetBorderTop(new DoubleBorder(2))
                .SetSplitCharacters(new NoSplitCharacters())
                .SetTextAlignment(TextAlignment.RIGHT)
                .Add(GetBigText(string.Format(CultureInfo.CurrentCulture, "{0:C}", total))
                    .SetBold()
                    .SetFontColor(_orange));

            productsTable.AddCell(totalColumn0Cell).AddCell(totalColumn1Cell).AddCell(totalColumn2Cell);

            doc.Add(productsTable);
        }
        private void AddTitle(Document doc, string text)
        {
            var mainTitle = GetBigText(text)
                .SetBold()
                .SetMarginBottom(20);
            doc.Add(mainTitle);
        }
        private void AddSubtitle(Document doc, string text)
        {
            var subTitle = GetMediumText(text)
                .SetBold()
                .SetMarginBottom(10);
            doc.Add(subTitle);
        }
        private void AddSignature(Document doc, string text, ProductsTransaction transaction)
        {
            var signature = new Text($"{text}: ______________________________")
                .SetFontSize(_regularFontSize)
                .SetBold();
            var locationAndDate = new Text(_localizer["{0} am {1} Uhr", transaction.Basar.Location, transaction.TimeStamp])
                .SetFontSize(_smallFontSize);

            var p = new Paragraph()
                .Add(signature)
                .Add(Environment.NewLine)
                .Add(locationAndDate);
            doc.Add(p);
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
        private static Paragraph GetRegularText(string text)
        {
            return new Paragraph(text)
                .SetFontSize(_regularFontSize);
        }
        private static Text GetSmallText(string text)
        {
            return new Text(text)
                .SetFontSize(_smallFontSize);
        }
        private static Paragraph GetSpacer(int verticalSize)
        {
            return new Paragraph()
                .SetMarginTop(verticalSize);
        }
    }
}
