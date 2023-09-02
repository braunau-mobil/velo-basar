using BraunauMobil.VeloBasar.BusinessLogic;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests;

public sealed class MemoryNumberService
    : INumberService
{
    private readonly Dictionary<int, Dictionary<TransactionType, int>> _numbers = new();

    public async Task<int> NextNumberAsync(int basarId, TransactionType transactionType)
    {
        if (!_numbers.ContainsKey(basarId))
        {
            _numbers.Add(basarId, new Dictionary<TransactionType, int>());
        }
        if (!_numbers[basarId].ContainsKey(transactionType))
        {
            _numbers[basarId].Add(transactionType, 1);
        }

        int result = _numbers[basarId][transactionType]++;
        return await Task.FromResult(result);
    }
}
