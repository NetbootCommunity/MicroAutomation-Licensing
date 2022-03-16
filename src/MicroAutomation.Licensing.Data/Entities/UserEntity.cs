#region Using

using MicroAutomation.Licensing.Data.Abstracts;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#endregion Using

namespace MicroAutomation.Licensing.Data.Entities;

/// <summary>
/// The user entity in the database.
/// </summary>
/// <seealso cref="MicroAutomation.Licensing.Data.Abstracts.ICreationTrackable" />
/// <seealso cref="MicroAutomation.Licensing.Data.Abstracts.IModificationTrackable" />
public class UserEntity : ICreationTrackable, IModificationTrackable
{
    #region Data

    public Guid Id { get; set; }
    public string DisplayName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }

    #endregion Data

    #region Navigation

    [JsonIgnore]
    public ICollection<UserOrganizationEntity> UserOrganizations { get; set; }

    #endregion Navigation

    #region Metadata

    public DateTimeOffset CreatedDataUtc { get; set; }
    public DateTimeOffset? UpdatedDataUtc { get; set; }

    #endregion Metadata
}