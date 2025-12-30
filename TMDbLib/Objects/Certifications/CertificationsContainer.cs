using System.Collections.Generic;

namespace TMDbLib.Objects.Certifications;

/// <summary>
/// Contains a collection of certifications organized by country code.
/// </summary>
public class CertificationsContainer
{
    /// <summary>
    /// Gets or sets the certifications dictionary, where the key is a country code and the value is a list of certification items for that country.
    /// </summary>
    public Dictionary<string, List<CertificationItem>?>? Certifications { get; set; }
}
