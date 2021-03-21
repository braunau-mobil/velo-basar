namespace BraunauMobil.VeloBasar.Models.Interfaces
{
    public interface IStateModel : IModel
    {
        ObjectState State { get; }
    }
}
