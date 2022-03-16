#region Using

using MicroAutomation.Licensing.Data.Abstracts;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#endregion Using

namespace MicroAutomation.Licensing.Data.Entities;

/// <summary>
/// The product entity in the database.
/// </summary>
/// <seealso cref="MicroAutomation.Licensing.Data.Abstracts.ICreationAuditable" />
public class ProductEntity : ICreationAuditable
{
    #region Data

    public Guid Id { get; set; }

    [JsonIgnore]
    public Guid OrganizationId { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public string Company { get; set; }

    [JsonIgnore]
    public string PassPhrase { get; set; }

    [JsonIgnore]
    public string PrivateKey { get; set; }

    [JsonIgnore]
    public string PublicKey { get; set; }

    #endregion Data

    #region Navigation

    [JsonIgnore]
    public OrganizationEntity Organization { get; set; }

    [JsonIgnore]
    public ICollection<LicenseEntity> Licenses { get; set; }

    #endregion Navigation

    #region Metadata

    public Guid CreatedDataBy { get; set; }
    public DateTimeOffset CreatedDataUtc { get; set; }

    #endregion Metadata
}