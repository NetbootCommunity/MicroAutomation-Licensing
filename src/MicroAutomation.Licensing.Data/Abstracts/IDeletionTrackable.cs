#region Using

using System;

#endregion Using

namespace MicroAutomation.Licensing.Data.Abstracts;

/// <summary>
/// An entity can implement this interface if <see cref="DeletedDataUtc" /> of this entity must be stored.
/// <see cref="DeletedDataUtc" /> is automatically set when deleting Entity.
/// </summary>
public interface IDeletionTrackable : ISoftDeletable
{
    DateTimeOffset? DeletedDataUtc { get; set; }
}