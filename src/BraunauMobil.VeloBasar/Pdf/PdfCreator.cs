using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BraunauMobil.VeloBasar.Models;
using iText.Barcodes;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace BraunauMobil.VeloBasar.Pdf
{
    public class PdfCreator
    {
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

        public byte[] CreateAcceptance(Basar basar, Acceptance acceptance)
        {
            return CreatePdf((pdfDoc, doc) =>
            {
                var page = pdfDoc.AddNewPage(PageSize.A5);

                doc.Add(new Paragraph($"Braunau mobil - {basar.Name}"));
                doc.Add(new Paragraph($"Annahme #{acceptance.Number}"));
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
                doc.Add(new Paragraph(product.Brand));
                doc.Add(new Paragraph(product.Type));
                doc.Add(new Paragraph(product.Description));
                doc.Add(new Paragraph($"Reifengröße: {product.TireSize}"));
                doc.Add(new Paragraph(product.Price.ToString()));

                var barcode = new BarcodeEAN(pdfDoc);
                barcode.SetCode($"{product.Id:0000000000000}");
                doc.Add(new Image(barcode.CreateFormXObject(pdfDoc)));
            });
        }

        public byte[] CreateSale(Basar basar, Sale sale)
        {
            return CreatePdf((pdfDoc, doc) =>
            {
                pdfDoc.SetDefaultPageSize(PageSize.A5);

                var p = new Paragraph($"Braunau, {sale.TimeStamp:dd.MM.yyyy}");
                p.SetFontSize(10);
                p.SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                doc.Add(p);

                p = new Paragraph($"Verkaufsbeleg: Braunau mobil: {basar.Name}");
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
                        .AddCell(product.Brand)
                        .AddCell(product.Type)
                        .AddCell($"{product.Price:C}");
                }
                doc.Add(table);
            });
        }

        public byte[] CreateSettlement(Basar basar, Settlement settlement)
        {
            return CreatePdf((pdfDoc, doc) =>
            {
                var page = pdfDoc.AddNewPage(PageSize.A5);

                doc.Add(new Paragraph($"Braunau mobil - {basar.Name}"));
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
    }
}
