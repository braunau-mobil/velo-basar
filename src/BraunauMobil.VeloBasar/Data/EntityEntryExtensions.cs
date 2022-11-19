using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BraunauMobil.VeloBasar.Data;

public static class EntityEntryExtensions
{
    public static IEnumerable<TEntity> AddedEntities<TEntity>(this IEnumerable<EntityEntry> entries)
        where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(entries);

        return entries.Entities<TEntity>(EntityState.Added);
    }

    public static IEnumerable<TEntity> ModifedEntities<TEntity>(this IEnumerable<EntityEntry> entries)
        where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(entries);

        return entries.Entities<TEntity>(EntityState.Modified);
    }

    public static IEnumerable<TEntity> Entities<TEntity>(this IEnumerable<EntityEntry> entries, EntityState state)
        where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(entries);

        return entries.Where(entry =>
                   entry.State == state
                && entry.Entity != null
                && entry.Entity is TEntity)
            .Select(entityEntry => (TEntity)entityEntry.Entity);
    }
}
