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

            return CreatePdf((pdfDoc, doc) =>
            {
                pdfDoc.SetDefaultPageSize(PageSize.A5);

                doc.SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN));
                doc.SetMargins(settings.PageMargins.Top, settings.PageMargins.Right, settings.PageMargins.Bottom, settings.PageMargins.Left);
                pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, new PageFooterHandler(doc, _localizer));


                var address = new Paragraph(acceptance.Seller.GetAddressText());
                address.SetFontSize(_regularFontSize);
                var locationAndDate = new Paragraph(acceptance.Basar.GetLocationAndDateText());
                locationAndDate.SetFontSize(_regularFontSize);
                var sellerId = new Paragraph(acceptance.Seller.GetIdText(_localizer));
                sellerId.SetFontSize(_regularFontSize);

                var headerTable = new Table(2);
                headerTable.UseAllAvailableWidth();
                var addressCell = new Cell(2, 1);
                addressCell.SetBorder(null);
                addressCell.Add(address);

                var locationAndDateCell = new Cell();
                locationAndDateCell.SetBorder(null);
                locationAndDateCell.SetTextAlignment(TextAlignment.RIGHT);
                locationAndDateCell.SetVerticalAlignment(VerticalAlignment.TOP);
                locationAndDateCell.Add(locationAndDate);

                var sellerIdCell = new Cell();
                sellerIdCell.SetBorder(null);
                sellerIdCell.SetTextAlignment(TextAlignment.RIGHT);
                sellerIdCell.SetVerticalAlignment(VerticalAlignment.BOTTOM);
                sellerIdCell.Add(sellerId);
                headerTable.AddCell(addressCell).AddCell(locationAndDateCell).AddCell(sellerIdCell);

                doc.Add(headerTable);

                var mainTitle = new Paragraph(string.Format(CultureInfo.CurrentCulture, settings.Acceptance.TitleFormat, acceptance.Basar.Name));
                mainTitle.SetFontSize(_bigFontSize);
                mainTitle.SetBold();
                mainTitle.SetMarginBottom(20);
                doc.Add(mainTitle);

                var subTitle = new Paragraph(settings.Acceptance.SubTitle);
                subTitle.SetFontSize(_mediumFontSize);
                subTitle.SetBold();
                subTitle.SetMarginBottom(10);
                doc.Add(subTitle);

                var productsTable = new Table(4);
                productsTable.UseAllAvailableWidth();
                productsTable.SetFontSize(_regularFontSize);

                var idHeader = new Paragraph(_localizer["Id"]);
                idHeader.SetBold();
                var idHeaderCell = new Cell();
                idHeaderCell.SetBorderTop(null);
                idHeaderCell.SetBorderLeft(null);
                idHeaderCell.SetTextAlignment(TextAlignment.CENTER);
                idHeaderCell.Add(idHeader);

                var productInfoHeader = new Paragraph(_localizer["Hersteller - Artikel\r\nZusatzinformationen"]);
                productInfoHeader.SetBold();
                var productInfoHeaderCell = new Cell();
                productInfoHeaderCell.SetBorderTop(null);
                productInfoHeaderCell.SetTextAlignment(TextAlignment.CENTER);
                productInfoHeaderCell.Add(productInfoHeader);

                var sizeHeader = new Paragraph(_localizer["Größe"]);
                sizeHeader.SetBold();
                var sizeHeaderCell = new Cell();
                sizeHeaderCell.SetBorderTop(null);
                sizeHeaderCell.SetTextAlignment(TextAlignment.CENTER);
                sizeHeaderCell.Add(sizeHeader);

                var priceHeader = new Paragraph(_localizer["Preis"]);
                priceHeader.SetBold();
                var priceHeaderCell = new Cell();
                priceHeaderCell.SetBorderTop(null);
                priceHeaderCell.SetBorderRight(null);
                priceHeaderCell.SetTextAlignment(TextAlignment.CENTER);
                priceHeaderCell.Add(priceHeader);

                productsTable.AddHeaderCell(idHeaderCell).AddHeaderCell(productInfoHeaderCell).AddHeaderCell(sizeHeaderCell).AddHeaderCell(priceHeaderCell);

                var sumFooter = new Paragraph(_localizer["Summe:"]);
                sumFooter.SetBold();
                var sumFooterCell = new Cell();
                sumFooterCell.SetBorderLeft(null);
                sumFooterCell.SetBorderTop(new DoubleBorder(2));
                sumFooterCell.SetBorderRight(null);
                sumFooterCell.SetBorderBottom(null);
                sumFooterCell.Add(sumFooter);

                var productInfoFooter = new Paragraph(_localizer["{0} Artikel", acceptance.Products.Count]);
                productInfoFooter.SetBold();
                var productInfoFooterCell = new Cell(1, 2);
                productInfoFooterCell.SetBorderLeft(null);
                productInfoFooterCell.SetBorderTop(new DoubleBorder(2));
                productInfoFooterCell.SetBorderRight(null);
                productInfoFooterCell.SetBorderBottom(null);
                productInfoFooterCell.Add(productInfoFooter);

                var priceFooter = new Paragraph(acceptance.GetSumText());
                priceFooter.SetBold();
                priceFooter.SetPaddingLeft(20);
                var priceFooterCell = new Cell();
                priceFooterCell.SetSplitCharacters(new NoSplitCharacters());
                priceFooterCell.SetBorderLeft(null);
                priceFooterCell.SetBorderTop(new DoubleBorder(2));
                priceFooterCell.SetBorderRight(null);
                priceFooterCell.SetBorderBottom(null);
                priceFooterCell.Add(priceFooter);

                productsTable.AddFooterCell(sumFooterCell).AddFooterCell(productInfoFooterCell).AddFooterCell(priceFooterCell);

                foreach (var product in acceptance.Products.Select(x => x.Product))
                {
                    var id = new Paragraph($"{product.Id}");
                    var idCell = new Cell();
                    idCell.SetBorderLeft(null);
                    idCell.SetBorderRight(null);
                    idCell.Add(id);

                    var productInfo = new Paragraph(product.GetInfoText(_localizer));
                    var productInfoCell = new Cell();
                    productInfoCell.SetBorderLeft(null);
                    productInfoCell.SetBorderRight(null);
                    productInfoCell.Add(productInfo);

                    var size = new Paragraph(product.TireSize);
                    var sizeCell = new Cell();
                    sizeCell.SetBorderLeft(null);
                    sizeCell.SetBorderRight(null);
                    sizeCell.SetTextAlignment(TextAlignment.RIGHT);
                    sizeCell.Add(size);

                    var price = new Paragraph(product.GetPriceText(acceptance));
                    var priceCell = new Cell();
                    priceCell.SetSplitCharacters(new NoSplitCharacters());
                    priceCell.SetBorderLeft(null);
                    priceCell.SetBorderRight(null);
                    priceCell.SetTextAlignment(TextAlignment.RIGHT);
                    price.SetKeepTogether(true);

                    priceCell.Add(price);

                    productsTable.AddCell(idCell).AddCell(productInfoCell).AddCell(sizeCell).AddCell(priceCell);
                }
                doc.Add(productsTable);

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

        public byte[] CreateSale(ProductsTransaction sale)
        {
            return CreatePdf((pdfDoc, doc) =>
            {
                pdfDoc.SetDefaultPageSize(PageSize.A5);

                var p = new Paragraph($"Braunau, {sale.TimeStamp:dd.MM.yyyy}");
                p.SetFontSize(10);
                p.SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                doc.Add(p);

                p = new Paragraph($"Verkaufsbeleg: Braunau mobil: {sale.Basar.Name}");
                p.SetFontSize(14);
                p.SetBold();
                doc.Add(p);

                var table = new Table(4);
                table.UseAllAvailableWidth();
                table.AddHeaderCell("PID");
                table.AddHeaderCell("Brand");
                table.AddHeaderCell("Type");
                table.AddHeaderCell("Price");
                foreach (var product in sale.Products.Select(ps => ps.Product))
                {
                    table.AddCell(product.Id.ToString())
                        .AddCell(product.Brand.Name)
                        .AddCell(product.Type.Name)
                        .AddCell($"{product.Price:C}");
                }
                doc.Add(table);
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
    }
}
