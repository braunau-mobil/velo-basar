using BraunauMobil.VeloBasar.Configuration;
using iText.Barcodes;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Extensions.Options;
using System.Globalization;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Pdf;

public sealed class ProductLabelService
    : IProductLabelService
{
    private readonly PdfGenerator _pdf;
    private readonly VeloTexts _txt;
    private readonly LabelPrintSettings _printSettings;

    public ProductLabelService(PdfGenerator pdf, VeloTexts txt, IOptions<PrintSettings> options)
    {
        _pdf = pdf ?? throw new ArgumentNullException(nameof(pdf));
        _txt = txt ?? throw new ArgumentNullException(nameof(txt));
        ArgumentNullException.ThrowIfNull(options);
        _printSettings = options.Value.Label;
    }

    public async Task<byte[]> CreateLabelAsync(ProductEntity product)
    {
        ArgumentNullException.ThrowIfNull(product);
        
        return await CreateLabelsAsync(new[] { product });
    }

    public async Task<byte[]> CreateLabelsAsync(IEnumerable<ProductEntity> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        byte[] data = _pdf.CreatePdf((pdfDoc, doc) =>
        {
            pdfDoc.SetDefaultPageSize(new PageSize(45f.ToUnit(), 79f.ToUnit()));
            doc.SetMargins(5, 5, 5, 5);
            doc.SetFontSize(10);
            doc.SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN));

            bool addPageBreak = false;
            foreach (ProductEntity product in products)
            {
                if (addPageBreak)
                {
                    doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                }

                AddProductLabel(pdfDoc, doc, product);

                addPageBreak = true;
            }
        });
        return await Task.FromResult(data);
    }

    private void AddProductLabel(PdfDocument pdfDoc, Document doc, ProductEntity product)
    {
        doc.Add(_pdf.GetRegularParagraph(string.Format(CultureInfo.CurrentCulture, _printSettings.TitleFormat, product.Session.Basar.Name))
                .SetTextAlignment(TextAlignment.CENTER)
                .SetBorderBottom(new SolidBorder(2)));

        Paragraph info = new Paragraph()
            .Add(_pdf.GetRegularText($"{product.Brand.Name} - {product.Type.Name}").SetBold())
            .Add(Environment.NewLine)
            .Add(_pdf.GetRegularText(product.Description.Truncate(_printSettings.MaxDescriptionLength)));

        if (product.Color != null)
        {
            info.Add(Environment.NewLine)
                .Add(_pdf.GetRegularText(product.Color));
        }

        if (product.FrameNumber != null)
        {
            info.Add(Environment.NewLine)
                .Add(_pdf.GetRegularText(_txt.FrameNumberLabel(product.FrameNumber)));
        }

        if (product.TireSize != null)
        {
            info.Add(Environment.NewLine)
                .Add(_pdf.GetRegularText(_txt.TireSizeLabel(product.TireSize)));
        }

        info.SetMargin(2);
        doc.Add(info);

        Paragraph barcodeAndPrice = new Paragraph()
            .SetTextAlignment(TextAlignment.CENTER);
        Barcode128 barcode = new (pdfDoc);
        barcode.SetCode($"{product.Id}");
        Paragraph price = _pdf.GetParagraph(14, string.Format(CultureInfo.CurrentCulture, "{0:C}", product.Price))
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
    }
}
