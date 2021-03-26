namespace BraunauMobil.VeloBasar.Pages.Generic
{
    public interface IEditPageModel
    {
        VeloPage ListPage(object parameter = null);
        VeloPage ListPageOrigin();
        object Item { get; }
    }
}
