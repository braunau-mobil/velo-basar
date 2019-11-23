namespace BraunauMobil.VeloBasar.Printing
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Converts milimeters into iText 7 units.
        /// This forumla was found somwhere in the internet.
        /// First ist converts mm into inch and then it multiplies it with 72. I think 72 is the maybe the DPI.
        /// </summary>
        public static float ToUnit(this float mm)
        {
            return (mm / 25.4f) * 72.0f;
        }
    }
}
