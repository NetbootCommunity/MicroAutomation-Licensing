#region Using

using MicroAutomation.Licensing.Data.Abstracts;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#endregion Using

namespace MicroAutomation.Licensing.Data.Entities;

/// <summary>
/// The organization entity in the database.
/// </summary>
/// <seealso cref="MicroAutomation.Licensing.Data.Abstracts.ICreationAuditable" />
public class OrganizationEntity : ICreationAuditable
{
    #region Data

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    #endregion Data

    #region Navigation

    [JsonIgnore]
    public ICollection<UserOrganizationEntity> UserOrganizations { get; set; }

    [JsonIgnore]
    public ICollection<ProductEntity> Products { get; set; }

    #endregion Navigation

    #region Metadata

    public Guid CreatedDataBy { get; set; }
    public DateTimeOffset CreatedDataUtc { get; set; }

    #endregion Metadata
}