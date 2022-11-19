namespace BraunauMobil.VeloBasar.Models.Entities;

public abstract class AbstractCrudEntity
    : AbstractEntity
    , ICrudEntity
{
    public ObjectState State { get; set; } = ObjectState.Enabled;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
