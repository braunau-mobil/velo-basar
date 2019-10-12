namespace BraunauMobil.VeloBasar
{
    public interface ISearchable : IBasarPage
    {
        string CurrentFilter { get; }
    }
}
