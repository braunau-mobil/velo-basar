namespace BraunauMobil.VeloBasar.Models.Entities;

#nullable disable warnings
public class AcceptSessionEntity
    : AbstractEntity
    , IEntity
{
    public AcceptSessionEntity()
    { }

    public AcceptSessionEntity(int basarId, SellerEntity seller, DateTime startTimeStamp)
    {
        ArgumentNullException.ThrowIfNull(seller);

        BasarId = basarId;
        Seller = seller;
        SellerId = seller.Id;
        StartTimeStamp = startTimeStamp;
    }

    public int BasarId { get; set; }

    public BasarEntity Basar { get; set; }

    public DateTime StartTimeStamp { get; set; }

    public DateTime? EndTimeStamp { get; set; }

    public AcceptSessionState State { get; set; } = AcceptSessionState.Uncompleted;

    public int SellerId { get; set; }

    public SellerEntity Seller { get; set; }

    public ICollection<ProductEntity> Products { get; } = new List<ProductEntity>();

    public bool CanAccept() => Products.Count > 0;

    public bool IsCompleted => EndTimeStamp.HasValue;

    public void Complete(DateTime timestamp)
    {
        EndTimeStamp = timestamp;
        Seller.ValueState = ValueState.NotSettled;
        State = AcceptSessionState.Completed;
    }
}
