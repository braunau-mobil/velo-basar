using BraunauMobil.VeloBasar.Configuration;
using iText.IO.Font.Constants;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Pdf;

public sealed class PageFooterHandler
    : IEventHandler
{
    public const float Height = 6;

    private readonly Margins _pageMargins;
    private readonly Document _doc;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public PageFooterHandler(Margins pageMargins, Document doc, IStringLocalizer<SharedResources> localizer)
    {
        _pageMargins = pageMargins ?? throw new ArgumentNullException(nameof(pageMargins));
        _doc = doc ?? throw new ArgumentNullException(nameof(doc));
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
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
            canvas.ShowText(_localizer[VeloTexts.PageNumberFromOverall, pageNumber, docEvent.GetDocument().GetNumberOfPages()]);
            canvas.ShowText("  - powered by https://github.com/braunau-mobil/velo-basar");
            canvas.EndText().Release();
        }
    }
}
