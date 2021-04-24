using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Resources;
using iText.IO.Font.Constants;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Printing
{
    public class PageFooterHandler : IEventHandler
    {
        public const float Height = 6;

        private readonly Margins _pageMargins;
        private readonly Document _doc;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public PageFooterHandler(Margins pageMargins, Document doc, IStringLocalizer<SharedResource> localizer)
        {
            _pageMargins = pageMargins;
            _doc = doc;
            _localizer = localizer;
        }

        public void HandleEvent(Event ev)
        {
            if (ev is PdfDocumentEvent docEvent)
            {
                var page = docEvent.GetPage();
                var pageNumber = docEvent.GetDocument().GetPageNumber(page);
                var canvas = new PdfCanvas(page);
                canvas.BeginText();
                canvas.SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA), Height);
                canvas.MoveText(_doc.GetLeftMargin(), _pageMargins.Bottom);
                canvas.ShowText(_localizer["Seite {0} von {1}", pageNumber, docEvent.GetDocument().GetNumberOfPages()]);
                canvas.ShowText("  - powered by https://github.com/braunau-mobil/velo-basar");
                canvas.EndText().Release();
            }
        }
    }
}
