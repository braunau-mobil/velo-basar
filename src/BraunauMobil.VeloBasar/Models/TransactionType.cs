namespace BraunauMobil.VeloBasar.Models;

public enum TransactionType
{
    Acceptance = 0,
    Sale = 1,
    Settlement = 2,
    Cancellation = 3,
    Lock = 4,
    SetLost = 5,
    Unlock = 6,
    Unsettlement = 7
};
