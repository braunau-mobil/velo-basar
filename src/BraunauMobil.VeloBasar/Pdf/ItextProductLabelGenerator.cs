using BraunauMobil.VeloBasar.Configuration;
using BraunauMobil.VeloBasar.Models.Documents;
using iText.Barcodes;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace BraunauMobil.VeloBasar.Pdf;

public sealed class ItextProductLabelGenerator(PdfGenerator pdf)
    : IProductLabelGenerator
{
    public async Task<byte[]> CreateLabelAsync(ProductLabelDocumentModel model, LabelPrintSettings settings)
    {
        ArgumentNullException.ThrowIfNull(model);
        
        return await CreateLabelsAsync([model], settings);
    }

    public async Task<byte[]> CreateLabelsAsync(IEnumerable<ProductLabelDocumentModel> models, LabelPrintSettings settings)
    {
        ArgumentNullException.ThrowIfNull(models);
        ArgumentNullException.ThrowIfNull(settings);

        byte[] data = pdf.CreatePdf((pdfDoc, doc) =>
        {
            pdfDoc.SetDefaultPageSize(new PageSize(settings.WidthInMillimeters.ToUnit(), settings.HeightInMillimeters.ToUnit()));
            doc.SetMargins(settings.Margins);
            doc.SetFontSize(10);
            doc.SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN));

            bool addPageBreak = false;
            foreach (ProductLabelDocumentModel model in models)
            {
                if (addPageBreak)
                {
                    doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                }

                AddProductLabel(pdfDoc, doc, model, settings);

                addPageBreak = true;
            }
        });
        return await Task.FromResult(data);
    }

    private void AddProductLabel(PdfDocument pdfDoc, Document doc, ProductLabelDocumentModel model, LabelPrintSettings settings)
    {
        doc.Add(pdf.GetRegularParagraph(model.Title)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetBorderBottom(new SolidBorder(2)));

        Paragraph info = new Paragraph()
            .Add(pdf.GetRegularText(model.BrandTypeInfo).SetBold())
            .Add(Environment.NewLine)
            .Add(pdf.GetRegularText(model.Description));

        if (model.Color != null)
        {
            info.Add(Environment.NewLine)
                .Add(pdf.GetRegularText(model.Color));
        }

        if (model.FrameNumber != null)
        {
            info.Add(Environment.NewLine)
                .Add(pdf.GetRegularText(model.FrameNumber));
        }

        if (model.TireSize != null)
        {
            info.Add(Environment.NewLine)
                .Add(pdf.GetRegularText(model.TireSize));
        }

        info.SetMargin(2);
        doc.Add(info);

        Paragraph barcodeAndPrice = new Paragraph()
            .SetTextAlignment(TextAlignment.CENTER);
        Barcode128 barcode = new (pdfDoc);
        barcode.SetCode(model.Barcode);
        Paragraph price = pdf.GetParagraph(14, model.Price)
            .SetBold()
            .SetTextAlignment(TextAlignment.RIGHT)
            .SetBorderTop(new SolidBorder(2))

            .SetWidth(settings.WidthInMillimeters.ToUnit());
        barcodeAndPrice
            .Add(new Image(barcode.CreateFormXObject(pdfDoc)))
            .Add(new Text(Environment.NewLine))
            .Add(price)
            .SetFixedPosition(settings.Margins.Left.ToUnit(), settings.Margins.Bottom.ToUnit(), GetContentWidthInMillimeters(settings).ToUnit());
        
        doc.Add(barcodeAndPrice);
    }

    private static int GetContentWidthInMillimeters(LabelPrintSettings settings)
        => settings.WidthInMillimeters - settings.Margins.Left - settings.Margins.Right;
}
