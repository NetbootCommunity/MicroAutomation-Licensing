#region Using

using System;

#endregion

namespace MicroAutomation.Licensing.Data.Abstracts;

/// <summary>
/// This interface is implemented by entities which wanted
/// to store deletion information (who and when deleted).
/// </summary>
public interface IDeletionAuditable : IDeletionTrackable
{
    Guid? DeletedDataBy { get; set; }
}