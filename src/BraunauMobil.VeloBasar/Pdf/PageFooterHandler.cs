using BraunauMobil.VeloBasar.Configuration;
using iText.IO.Font.Constants;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;

namespace BraunauMobil.VeloBasar.Pdf;

public sealed class PageFooterHandler
    : IEventHandler
{
    public const float Height = 6;

    private readonly Margins _pageMargins;
    private readonly Document _doc;
    private readonly VeloTexts _txt;

    public PageFooterHandler(Margins pageMargins, Document doc, VeloTexts txt)
    {
        _pageMargins = pageMargins ?? throw new ArgumentNullException(nameof(pageMargins));
        _doc = doc ?? throw new ArgumentNullException(nameof(doc));
        _txt = txt ?? throw new ArgumentNullException(nameof(txt));
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
            canvas.ShowText(_txt.PageNumberFromOverall(pageNumber, docEvent.GetDocument().GetNumberOfPages()));
            canvas.ShowText("  - powered by https://github.com/braunau-mobil/velo-basar");
            canvas.EndText().Release();
        }
    }
}
