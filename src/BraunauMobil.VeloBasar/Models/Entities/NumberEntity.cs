namespace BraunauMobil.VeloBasar.Models.Entities;

#nullable disable warnings
public sealed class NumberEntity
{
    public int BasarId { get; set; }

    public BasarEntity Basar { get; set; }

    public TransactionType Type { get; set; }

    public int Value { get; set; }
}
