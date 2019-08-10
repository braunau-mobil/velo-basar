using System;
using System.Collections.Generic;
using System.IO;
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

        public byte[] CreateAcceptance(Acceptance acceptance)
        {
            throw new NotImplementedException();
        }

        public byte[] CreateLabel(Basar basar, Product product)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                using (var pdfWriter = new PdfWriter(memoryStream))
                using (var pdfDoc = new PdfDocument(pdfWriter))
                using (var doc = new Document(pdfDoc))
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
                }

                bytes = memoryStream.GetBuffer();
            }
            return bytes;
        }

        public byte[] CreateSettlement(Settlement settlement)
        {
            throw new NotImplementedException();
        }
    }
}
