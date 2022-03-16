namespace MicroAutomation.Licensing.Data.Abstracts;

/// <summary>
/// Used to standardize soft deleting entities. Soft-delete entities are not actually deleted,
/// marked as IsDeleted = true in the database, but can not be retrieved to the application.
/// </summary>
public interface ISoftDeletable
{
    bool IsDeletedData { get; set; }
}