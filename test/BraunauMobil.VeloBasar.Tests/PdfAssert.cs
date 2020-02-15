using iText.Kernel.Pdf;
using System.IO;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests
{
    public static class PdfAssert
    {
        public static void PageCount(int expectedPageCount, byte[] file)
        {
            using var pfdStream = new MemoryStream(file);
            using var pdfReader = new PdfReader(pfdStream);
            using var pdfDoc = new PdfDocument(pdfReader);
            Assert.Equal(expectedPageCount, pdfDoc.GetNumberOfPages());
        }
    }
}
