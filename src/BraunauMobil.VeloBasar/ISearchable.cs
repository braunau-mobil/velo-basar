namespace BraunauMobil.VeloBasar
{
    public interface ISearchable
    {
        string SearchString { get; }
        VeloPage GetSearchPage();
    }
}
