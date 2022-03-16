#region Using

using MicroAutomation.Licensing.Data.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;

#endregion

namespace MicroAutomation.Licensing.Data.Shared.Extensions;

internal static partial class DbContextExtensions
{
    /// <summary>
    /// Populate special properties for all Trackable Entities in context.
    /// </summary>
    public static void UpdateTrackableEntities(this DbContext context)
    {
        DateTimeOffset utcNow = DateTimeOffset.UtcNow;

        var changedEntries = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added
                     || e.State == EntityState.Modified
                     || e.State == EntityState.Deleted);

        foreach (var dbEntry in changedEntries)
        {
            UpdateTrackableEntity(dbEntry, utcNow);
        }
    }

    private static void UpdateTrackableEntity(EntityEntry dbEntry, DateTimeOffset utcNow)
    {
        object entity = dbEntry.Entity;

        switch (dbEntry.State)
        {
            case EntityState.Added:
                if (entity is ICreationTrackable creationTrackable)
                {
                    creationTrackable.CreatedDataUtc = utcNow;
                }
                break;

            case EntityState.Modified:
                if (entity is IModificationTrackable modificatonTrackable)
                {
                    modificatonTrackable.UpdatedDataUtc = utcNow;
                    dbEntry.CurrentValues[nameof(IModificationTrackable.UpdatedDataUtc)] = utcNow;

                    if (entity is ICreationTrackable)
                    {
                        PreventPropertyOverwrite<DateTime>(dbEntry, nameof(ICreationTrackable.CreatedDataUtc));
                    }
                }
                break;

            case EntityState.Deleted:
                if (entity is ISoftDeletable softDeletable)
                {
                    dbEntry.State = EntityState.Unchanged;
                    softDeletable.IsDeletedData = true;
                    dbEntry.CurrentValues[nameof(ISoftDeletable.IsDeletedData)] = true;

                    if (entity is IDeletionTrackable deletionTrackable)
                    {
                        deletionTrackable.DeletedDataUtc = utcNow;
                        dbEntry.CurrentValues[nameof(IDeletionTrackable.DeletedDataUtc)] = utcNow;
                    }
                }
                break;

            default:
                throw new NotSupportedException();
        }
    }

    /// <summary>
    /// If we set <see cref="EntityEntry.State"/> to <see cref="EntityState.Modified"/> on entity with
    /// empty <see cref="ICreationTrackable.CreatedUtc"/> or <see cref="ICreationAuditable.CreatorUserId"/>
    /// we should not overwrite database values.
    /// https://github.com/gnaeus/EntityFramework.CommonTools/issues/4
    /// </summary>
    private static void PreventPropertyOverwrite<TProperty>(EntityEntry dbEntry, string propertyName)
    {
        var propertyEntry = dbEntry.Property(propertyName);

        if (propertyEntry.IsModified && Equals(dbEntry.CurrentValues[propertyName], default(TProperty)))
        {
            propertyEntry.IsModified = false;
        }
    }
}