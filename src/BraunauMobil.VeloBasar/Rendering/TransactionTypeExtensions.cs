using System.Diagnostics;

namespace BraunauMobil.VeloBasar.Rendering;

public static class TransactionTypeExtensions
{
    public static string ToCssColor(this TransactionType transactionType)
        => transactionType switch
        {
            TransactionType.Acceptance => "light",
            TransactionType.Cancellation => "info",
            TransactionType.Lock => "danger",
            TransactionType.SetLost => "warning",
            TransactionType.Unlock => "secondary",
            TransactionType.Sale => "success",
            TransactionType.Settlement => "secondary",
            _ => throw new UnreachableException(),
        };
}
