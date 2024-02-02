using BraunauMobil.VeloBasar.Configuration;
using iText.IO.Font.Constants;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using System.Globalization;

namespace BraunauMobil.VeloBasar.Pdf;

public sealed class PageFooterHandler
    : IEventHandler
{
    public const float Height = 6;

    private readonly Margins _pageMargins;
    private readonly Document _doc;
    private readonly string _pageNumberFormat;
    private readonly string _poweredBy;

    public PageFooterHandler(Margins pageMargins, Document doc, string pageNumberFormat, string poweredBy)
    {
        _pageMargins = pageMargins ?? throw new ArgumentNullException(nameof(pageMargins));
        _doc = doc ?? throw new ArgumentNullException(nameof(doc));
        _pageNumberFormat = pageNumberFormat ?? throw new ArgumentNullException(nameof(pageNumberFormat));
        _poweredBy = poweredBy ?? throw new ArgumentNullException(nameof(poweredBy));
    }

    public void HandleEvent(Event @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        if (@event is PdfDocumentEvent docEvent)
        {
            PdfPage page = docEvent.GetPage();
            int pageNumber = docEvent.GetDocument().GetPageNumber(page);
            PdfCanvas canvas = new(page);
            canvas.BeginText();
            canvas.SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA), Height);
            canvas.MoveText(_doc.GetLeftMargin(), _pageMargins.Bottom);
            canvas.ShowText(string.Format(CultureInfo.CurrentCulture, _pageNumberFormat, pageNumber, docEvent.GetDocument().GetNumberOfPages()));
            canvas.ShowText(_poweredBy);
            canvas.EndText().Release();
        }
    }
}
