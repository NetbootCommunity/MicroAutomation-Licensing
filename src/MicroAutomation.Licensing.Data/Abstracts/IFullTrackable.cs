namespace MicroAutomation.Licensing.Data.Abstracts;

/// <summary>
/// This interface is implemented by entities which modification times must be tracked.
/// Related properties automatically set when saving/updating/deleting Entity objects.
/// </summary>
public interface IFullTrackable : ICreationTrackable, IModificationTrackable, IDeletionTrackable
{ }