using iText.IO.Font.Otf;
using iText.Layout.Splitting;

namespace BraunauMobil.VeloBasar.Printing
{
    public class NoSplitCharacters : ISplitCharacters
    {
        public bool IsSplitCharacter(GlyphLine text, int glyphPos) => false;
    }
}
