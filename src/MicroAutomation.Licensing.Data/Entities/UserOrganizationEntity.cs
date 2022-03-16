#region Using

using MicroAutomation.Licensing.Data.Abstracts;
using MicroAutomation.Licensing.Data.Enum;
using System;
using System.Text.Json.Serialization;

#endregion Using

namespace MicroAutomation.Licensing.Data.Entities;

public class UserOrganizationEntity : ICreationAuditable
{
    #region Data

    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid UserId { get; set; }
    public OrganizationRole Role { get; set; }

    #endregion Data

    #region Navigation

    [JsonIgnore]
    public OrganizationEntity Organization { get; set; }

    [JsonIgnore]
    public UserEntity User { get; set; }

    #endregion Navigation

    #region Metadata

    public Guid CreatedDataBy { get; set; }
    public DateTimeOffset CreatedDataUtc { get; set; }

    #endregion Metadata
}