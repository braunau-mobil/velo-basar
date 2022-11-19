using Microsoft.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Data;

public static class NumberExtensions
{
    public static async Task<IReadOnlyList<NumberEntity>> GetForBasarAsync(this IQueryable<NumberEntity> numbers, int basarId)
    {
        ArgumentNullException.ThrowIfNull(numbers);

        return await numbers.Where(n => n.BasarId == basarId)
            .ToArrayAsync();
    }
}
