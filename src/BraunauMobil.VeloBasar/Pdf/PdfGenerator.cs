using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iColor = iText.Kernel.Colors.Color;

namespace BraunauMobil.VeloBasar.Pdf;

public sealed class PdfGenerator
{
    private const int _bigFontSize = 12;
    private const int _mediumFontSize = 10;
    private const int _smallFontSize = 6;
    private const int _regularFontSize = 8;

    public PdfGenerator()
    {
        Green = iColor.MakeColor(ColorConstants.GREEN.GetColorSpace(), new float[] { 0f, 0.5f, 0f });
        Orange = iColor.MakeColor(ColorConstants.GREEN.GetColorSpace(), new float[] { 1f, 0.5f, 0f });
    }

    public int RegularFontSize { get => _regularFontSize; }

    public int SmallFontSize { get => _smallFontSize; }

    public iColor Green { get; }

    public iColor Orange { get; }

    public byte[] CreatePdf(Action<PdfDocument, Document> decorate)
    {
        ArgumentNullException.ThrowIfNull(decorate);

        byte[] bytes;
        using (MemoryStream memoryStream = new())
        {
            using (PdfWriter pdfWriter = new(memoryStream))
            using (PdfDocument pdfDoc = new(pdfWriter))
            using (Document doc = new(pdfDoc))
            {
                decorate(pdfDoc, doc);
            }

            bytes = memoryStream.GetBuffer();
        }
        return bytes;
    }

    public void AddBannerSubtitle(Document doc, string bannerSubtitleText, string websiteText)
    {
        ArgumentNullException.ThrowIfNull(doc);
        ArgumentNullException.ThrowIfNull(bannerSubtitleText);
        ArgumentNullException.ThrowIfNull(websiteText);

        Paragraph bannerSubtitle = GetRegularParagraph(bannerSubtitleText)
            .SetMargin(0)
            .SetTextAlignment(TextAlignment.CENTER);
        doc.Add(bannerSubtitle);

        Paragraph website = GetRegularParagraph(websiteText)
            .SetMarginTop(0)
            .SetMarginBottom(10)
            .SetFontColor(Green)
            .SetTextAlignment(TextAlignment.CENTER);
        doc.Add(website);
    }    

    public void AddHeader(Document doc, string? addressText, string? locationAndDateText, string? sellerIdText)
    {
        ArgumentNullException.ThrowIfNull(doc);

        Table headerTable = new(2);
        headerTable.UseAllAvailableWidth();

        Cell addressCell = new Cell(2, 1)
            .SetBorder(null);
        if (addressText != null)
        {
            addressCell.Add(GetRegularParagraph(addressText));
        }

        Cell locationAndDateCell = new Cell()
            .SetBorder(null)
            .SetTextAlignment(TextAlignment.RIGHT)
            .SetVerticalAlignment(VerticalAlignment.TOP);
        if (locationAndDateText != null)
        {
            locationAndDateCell.Add(GetRegularParagraph(locationAndDateText));
        }

        Cell sellerIdCell = new Cell()
            .SetBorder(null)
            .SetTextAlignment(TextAlignment.RIGHT)
            .SetVerticalAlignment(VerticalAlignment.BOTTOM);
        if (sellerIdText != null)
        {
            sellerIdCell.Add(GetRegularParagraph(sellerIdText));
        }

        headerTable.AddCell(addressCell).AddCell(locationAndDateCell).AddCell(sellerIdCell);
        doc.Add(headerTable);
    }

    public void AddTitle(Document doc, string text)
    {
        ArgumentNullException.ThrowIfNull(doc);
        ArgumentNullException.ThrowIfNull(text);

        Paragraph mainTitle = GetBigParagraph(text)
            .SetBold()
            .SetMarginBottom(10);
        doc.Add(mainTitle);
    }

    public void AddSubtitle(Document doc, string text)
    {
        ArgumentNullException.ThrowIfNull(doc);
        ArgumentNullException.ThrowIfNull(text);
        Paragraph subTitle = GetMediumParagraph(text)
            .SetBold()
            .SetMarginBottom(10);
        doc.Add(subTitle);
    }

    public Paragraph GetParagraph(int fontSize, string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        return new Paragraph(text)
            .SetFontSize(fontSize);
    }

    public Text GetText(int fontSize, string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        return new Text(text)
            .SetFontSize(fontSize);
    }

    public Paragraph GetBigParagraph(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        return GetParagraph(_bigFontSize, text);
    }

    public Paragraph GetMediumParagraph(string text)
            {
        ArgumentNullException.ThrowIfNull(text);

        return GetParagraph(_mediumFontSize, text);
    }

    public Paragraph GetRegularParagraph(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        return GetParagraph(_regularFontSize, text);
    }

    public Text GetSmallText(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        return GetText(_smallFontSize, text);
    }

    public Text GetRegularText(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        return GetText(_regularFontSize, text);
    }

    public Paragraph GetBoldParagraph(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        return new Paragraph(text)
            .SetBold();
    }
    public Paragraph GetSpacer(int verticalSize)
       => new Paragraph()
            .SetMarginTop(verticalSize);
}
