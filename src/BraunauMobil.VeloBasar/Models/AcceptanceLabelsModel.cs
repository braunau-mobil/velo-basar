namespace BraunauMobil.VeloBasar.Models;

public sealed class AcceptanceLabelsModel
    : AbstractActiveBasarModel
{
    public int Number { get; set; }

    public int Id { get; set; }

    public bool OpenDocument { get; set; }
}
