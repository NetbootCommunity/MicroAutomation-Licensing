#region Using

using System;

#endregion Using

namespace MicroAutomation.Licensing.Data.Abstracts;

/// <summary>
/// An entity can implement this interface if <see cref="UpdatedDataUtc" /> of this entity must be stored.
/// <see cref="UpdatedDataUtc" /> is automatically set when updating Entity.
/// </summary>
public interface IModificationTrackable
{
    DateTimeOffset? UpdatedDataUtc { get; set; }
}