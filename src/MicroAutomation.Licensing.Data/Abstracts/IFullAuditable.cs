namespace MicroAutomation.Licensing.Data.Abstracts;

/// <summary>
/// This interface is implemented by entities which must be audited.
/// Related properties automatically set when saving/updating/deleting Entity objects.
/// </summary>
public interface IFullAuditable : IFullTrackable, ICreationAuditable, IModificationAuditable, IDeletionAuditable
{ }