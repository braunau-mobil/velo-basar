using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Configuration;

public sealed class Margins
{
    public Margins()
    { }

    public Margins(int left, int top, int right, int bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    [Required]
    public int Left { get; set; }

    [Required]
    public int Top { get; set; }

    [Required]
    public int Right { get; set; }

    [Required]
    public int Bottom { get; set; }
}
