using BraunauMobil.VeloBasar.Configuration;
using iText.Layout;

namespace BraunauMobil.VeloBasar.Pdf;

public static class PdfExtensions
{
    /// <summary>
    /// Converts milimeters into iText 7 units.
    /// This forumla was found somwhere in the internet.
    /// First ist converts mm into inch and then it multiplies it with 72. I think 72 is the maybe the DPI.
    /// </summary>
    public static float ToUnit(this int mm)
        => ToUnit((float)mm);

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public static float ToUnit(this float mm)
    {
        return (mm / 25.4f) * 72.0f;
    }

    public static void SetMargins(this Document doc, Margins margins)
    {
        ArgumentNullException.ThrowIfNull(margins);
        ArgumentNullException.ThrowIfNull(doc);
        
        doc.SetMargins(margins.Top.ToUnit(), margins.Right.ToUnit(), margins.Bottom.ToUnit(), margins.Left.ToUnit());
    }
}
