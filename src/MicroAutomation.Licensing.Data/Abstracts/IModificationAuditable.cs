#region Using

using System;

#endregion Using

namespace MicroAutomation.Licensing.Data.Abstracts;

/// <summary>
/// This interface is implemented by entities that is wanted
/// to store modification information (who and when modified lastly).
/// Properties are automatically set when updating the Entity.
/// </summary>
public interface IModificationAuditable : IModificationTrackable
{
    Guid? UpdatedDataBy { get; set; }
}