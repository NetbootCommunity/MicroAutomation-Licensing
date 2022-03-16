#region Using

using MicroAutomation.Licensing.Data.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;

#endregion Using

namespace MicroAutomation.Licensing.Data.Shared.Extensions;

internal static partial class DbContextExtensions
{
    /// <summary>
    /// Populate special properties for all Auditable Entities in context.
    /// </summary>
    public static void UpdateAuditableEntities(this DbContext context, Guid editorUserId)
    {
        DateTime utcNow = DateTime.UtcNow;

        var changedEntries = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added
                     || e.State == EntityState.Modified
                     || e.State == EntityState.Deleted);

        foreach (var dbEntry in changedEntries)
        {
            UpdateAuditableEntity(dbEntry, utcNow, editorUserId);
        }
    }

    private static void UpdateAuditableEntity(
        EntityEntry dbEntry, DateTime utcNow, Guid editorUserId)
    {
        object entity = dbEntry.Entity;

        switch (dbEntry.State)
        {
            case EntityState.Added:

                if (entity is ICreationAuditable creationAuditable)
                {
                    UpdateTrackableEntity(dbEntry, utcNow);
                    creationAuditable.CreatedDataBy = editorUserId;
                }
                break;

            case EntityState.Modified:
                if (entity is IModificationAuditable modificationAuditable)
                {
                    UpdateTrackableEntity(dbEntry, utcNow);
                    modificationAuditable.UpdatedDataBy = editorUserId;
                    dbEntry.CurrentValues[nameof(IModificationAuditable.UpdatedDataBy)] = editorUserId;

                    if (entity is ICreationAuditable)
                    {
                        PreventPropertyOverwrite<string>(dbEntry, nameof(ICreationAuditable.CreatedDataBy));
                    }
                }
                break;

            case EntityState.Deleted:
                if (entity is IDeletionAuditable deletionAuditable)
                {
                    UpdateTrackableEntity(dbEntry, utcNow);

                    // change CurrentValues after dbEntry.State becomes EntityState.Unchanged
                    deletionAuditable.DeletedDataBy = editorUserId;
                    dbEntry.CurrentValues[nameof(IDeletionAuditable.DeletedDataBy)] = editorUserId;
                }
                break;

            default:
                throw new NotSupportedException();
        }
    }
}