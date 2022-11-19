using System.Diagnostics;

namespace BraunauMobil.VeloBasar.Models;

public sealed class ChangeInfo
{
    private static readonly IReadOnlyList<decimal> _nominations = new[]
    {
        500m,
        200m,
        100m,
        50m,
        20m,
        10m,
        5m,
        2m,
        1m,
        0.5m,
        0.2m,
        0.1m,
        0.05m,
        0.02m,
        0.01m
    };

    public static ChangeInfo CreateFor(TransactionEntity transaction, decimal amountGiven) 
    {
        ArgumentNullException.ThrowIfNull(transaction);

        switch (transaction.Type)
        {
            case TransactionType.Acceptance:
            case TransactionType.Lock:
            case TransactionType.SetLost:
            case TransactionType.Unlock:
                return new ChangeInfo();
            case TransactionType.Cancellation:
                return new ChangeInfo(transaction.GetProductsSum());
            case TransactionType.Settlement:
                return new ChangeInfo(transaction.GetSoldTotal());
            case TransactionType.Sale:
                {
                    decimal sum = transaction.GetSoldProductsSum();
                    if (amountGiven < sum)
                    {
                        return new ChangeInfo();
                    }
                    return new ChangeInfo(amountGiven - sum);
                }
            default:
                throw new UnreachableException();
        }
    }

    public ChangeInfo()
    { }

    public ChangeInfo(decimal amount)
    {
        Amount = amount;
        IsValid = true;

        Dictionary<decimal, int> denomination = new ();
        decimal remainingAmount = Amount;
        foreach (decimal nomination in _nominations.OrderByDescending(x => x).Distinct())
        {
            if (nomination > Amount)
            {
                denomination.Add(nomination, 0);
                continue;
            }

            int count = (int)(remainingAmount / nomination);
            denomination.Add(nomination, count);

            remainingAmount -= (nomination * count);
        }
        Denomination = denomination;
    }

    public decimal Amount { get; init; }

    public bool IsValid { get; }

    public bool HasDenomination { get => Denomination.Values.Any(x => x > 0); }

    public IReadOnlyDictionary<decimal, int> Denomination { get; } = new Dictionary<decimal, int>();
}
