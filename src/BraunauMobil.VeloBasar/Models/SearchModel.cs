namespace BraunauMobil.VeloBasar.Models;

public sealed class SearchModel
{
    public SearchModel(string? searchString, string resetUrl)
    {
        SearchString = searchString;
        ResetUrl = resetUrl ?? throw new ArgumentNullException(nameof(resetUrl));
    }

    public string? SearchString { get; }

    public string ResetUrl { get; }    
}
