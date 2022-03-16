#region Using

using System;

#endregion Using

namespace MicroAutomation.Licensing.Data.Abstracts;

/// <summary>
/// An entity can implement this interface if <see cref="CreatedDataUtc" /> of this entity must be stored.
/// <see cref="CreatedDataUtc" /> is automatically set when saving Entity to database.</summary>
public interface ICreationTrackable
{
    DateTimeOffset CreatedDataUtc { get; set; }
}