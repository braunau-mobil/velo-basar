using System;

namespace BraunauMobil.VeloBasar
{
    public interface ISearchable
    {
        string CurrentFilter { get; }
        
        VeloPage GetSearchPage();
    }
}
