namespace BraunauMobil.VeloBasar.Pages.Generic
{
    public interface ICreatePageModel
    {
        VeloPage ListPage(object parameter = null);
        object Item { get; }
    }
}
