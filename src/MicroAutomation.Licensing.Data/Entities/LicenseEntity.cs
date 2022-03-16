#region Using

using MicroAutomation.Licensing.Data.Abstracts;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#endregion Using

namespace MicroAutomation.Licensing.Data.Entities;

/// <summary>
/// The license entity in the database.
/// </summary>
/// <seealso cref="MicroAutomation.Licensing.Data.Abstracts.ICreationAuditable" />
public class LicenseEntity : ICreationAuditable
{
    #region Data

    public Guid Id { get; set; }

    [JsonIgnore]
    public Guid ProductId { get; set; }

    public string Name { get; set; }
    public string Email { get; set; }
    public string Company { get; set; }
    public string Type { get; set; }
    public DateTime ExpiresAt { get; set; }
    public Dictionary<string, string> AdditionalAttributes { get; set; }

    public Dictionary<string, string> ProductFeatures { get; set; }

    #endregion Data

    #region Navigation

    [JsonIgnore]
    public ProductEntity Product { get; set; }

    #endregion Navigation

    #region Metadata

    public Guid CreatedDataBy { get; set; }
    public DateTimeOffset CreatedDataUtc { get; set; }

    #endregion Metadata
}