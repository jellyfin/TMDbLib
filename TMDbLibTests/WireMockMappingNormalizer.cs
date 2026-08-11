using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace TMDbLibTests;

/// <summary>
/// Normalizes WireMock mapping files to use deterministic IDs based on request signatures.
/// This ensures that re-recording produces the same filenames when requests are identical.
/// </summary>
public static class WireMockMappingNormalizer
{
    /// <summary>
    /// Response headers that differ between recordings without carrying any information the
    /// tests rely on (CDN routing, cache bookkeeping, content hashes). Keeping them would make
    /// every refresh of the recorded data churn even where the payload is byte-identical.
    /// </summary>
    private static readonly string[] _dynamicHeaders =
    [
        "Age",
        "Alt-Svc",
        "Cache-Control",
        "Date",
        "ETag",
        "Vary",
        "Via",
        "X-Amz-Cf-Id",
        "X-Amz-Cf-Pop",
        "X-Cache",
        "X-Gateway-Cache-Status",
        "x-az",
        "x-memc",
        "x-memc-age",
        "x-memc-expires",
        "x-memc-key",
        "x-task-id"
    ];

    /// <summary>
    /// Request paths whose response body is an unordered lookup table.
    /// </summary>
    private static readonly HashSet<string> _unorderedResponsePaths = new(StringComparer.OrdinalIgnoreCase)
    {
        "/3/certification/movie/list",
        "/3/certification/tv/list",
        "/3/configuration/countries",
        "/3/configuration/jobs",
        "/3/configuration/languages",
        "/3/configuration/primary_translations",
        "/3/configuration/timezones",
        "/3/watch/providers/regions"
    };

    /// <summary>
    /// Credentials TMDb mints per call. Lengths match the wire shape. The expiry stays in the
    /// past, like any recorded expiry, and after the lower bound
    /// <c>CustomDatetimeFormatConverterTest</c> allows - that test range-checks the value, so a
    /// far-future pin would fail its upper bound of two days out.
    /// </summary>
    private const string PinnedRequestToken = "0000000000000000000000000000000000000000";
    private const string PinnedGuestSessionId = "00000000000000000000000000000000";
    private const string PinnedExpiry = "2026-01-01 00:00:00 UTC";

    /// <summary>
    /// Body fields that hold a live counter or a minted credential rather than anything the
    /// tests depend on, mapped to the value each is pinned to. Prototypes rather than literals
    /// so the numbers keep their wire shape - System.Text.Json writes the shortest
    /// round-trippable form, turning an assigned 0.0 back into 0. Deliberately excludes
    /// <c>rating</c>, which looks similar to the counters but is load-bearing.
    /// </summary>
    private static readonly Dictionary<string, JsonNode> _volatileBodyFields = new(StringComparer.OrdinalIgnoreCase)
    {
        ["popularity"] = JsonNode.Parse("0.0")!,
        ["vote_average"] = JsonNode.Parse("0.0")!,
        ["vote_count"] = JsonNode.Parse("0")!,
        ["expires_at"] = JsonValue.Create(PinnedExpiry),
        ["guest_session_id"] = JsonValue.Create(PinnedGuestSessionId),
        ["request_token"] = JsonValue.Create(PinnedRequestToken),
        ["session_id"] = JsonValue.Create(PinnedRequestToken)
    };

    /// <summary>
    /// Response headers that echo a minted credential. They cannot just be dropped like the
    /// dynamic ones, because a test asserts the callback is present.
    /// </summary>
    private static readonly Dictionary<string, string> _pinnedHeaders = new(StringComparer.OrdinalIgnoreCase)
    {
        ["authentication-callback"] = $"https://www.themoviedb.org/authenticate/{PinnedRequestToken}"
    };

    /// <summary>
    /// Strips volatile bookkeeping from a recorded mapping, pins its live counters and
    /// canonicalises the element order of unordered response bodies, so that re-recording
    /// unchanged data yields an unchanged file.
    /// </summary>
    /// <param name="node">The parsed mapping to normalize in place.</param>
    public static void NormalizeMapping(JsonNode node)
    {
        // WireMock stamps the record time into every mapping; it alone would dirty all files.
        if (node is JsonObject root)
        {
            root.Remove("UpdatedAt");
        }

        NormalizeHeaders(node);
        ScrubVolatileBodyFields(node["Response"]?["BodyAsJson"]);
        SortUnorderedResponseBody(node);
    }

    /// <summary>
    /// Normalizes all mapping files in the specified directory.
    /// - Generates deterministic GUIDs based on request signature (method + path + sorted params)
    /// - Renames files to use the deterministic GUID
    /// - Updates the Guid field inside the JSON
    /// - Removes duplicate mappings for the same request signature
    /// </summary>
    /// <param name="mappingsDirectory">Path to the WireMock mappings directory</param>
    /// <returns>Summary of changes made</returns>
    public static NormalizationResult Normalize(string mappingsDirectory)
    {
        var result = new NormalizationResult();
        var mappingFiles = Directory.GetFiles(mappingsDirectory, "*.json");
        var seenSignatures = new Dictionary<string, string>();

        foreach (var file in mappingFiles)
        {
            try
            {
                var json = File.ReadAllText(file);
                var node = JsonNode.Parse(json);
                if (node is null) continue;

                var signature = ExtractRequestSignature(node);
                if (signature is null)
                {
                    result.Skipped.Add(file);
                    continue;
                }

                var deterministicGuid = GenerateDeterministicGuid(signature);

                // Check for duplicates
                if (seenSignatures.TryGetValue(signature, out var existingFile))
                {
                    // Delete duplicate
                    File.Delete(file);
                    result.Duplicates.Add((file, existingFile));
                    continue;
                }

                seenSignatures[signature] = file;

                // Update the JSON with new GUID
                var oldGuid = node["Guid"]?.GetValue<string>() ?? string.Empty;
                node["Guid"] = deterministicGuid;

                // Same treatment the record path applies, so the two cannot drift apart
                NormalizeMapping(node);

                var updatedJson = node.ToJsonString(new JsonSerializerOptions { WriteIndented = true });

                // Calculate new filename
                var fileName = Path.GetFileName(file);
                var newFileName = ReplaceGuidInFileName(fileName, oldGuid, deterministicGuid);
                var newFilePath = Path.Combine(mappingsDirectory, newFileName);

                // Write updated content
                if (file != newFilePath)
                {
                    File.WriteAllText(newFilePath, updatedJson);
                    File.Delete(file);
                    result.Renamed.Add((file, newFilePath));
                }
                else
                {
                    File.WriteAllText(file, updatedJson);
                    result.Updated.Add(file);
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add((file, ex.Message));
            }
        }

        return result;
    }

    private static string? ExtractRequestSignature(JsonNode node)
    {
        var request = node["Request"];
        if (request is null) return null;

        var sb = new StringBuilder();

        // Method
        var methods = request["Methods"]?.AsArray();
        if (methods is not null && methods.Count > 0)
        {
            sb.Append(methods[0]?.GetValue<string>() ?? "GET");
        }
        else
        {
            sb.Append("GET");
        }
        sb.Append('|');

        // Path
        var pathMatchers = request["Path"]?["Matchers"]?.AsArray();
        if (pathMatchers is not null && pathMatchers.Count > 0)
        {
            sb.Append(pathMatchers[0]?["Pattern"]?.GetValue<string>() ?? "");
        }
        sb.Append('|');

        // Query parameters (sorted for consistency)
        var parameters = new SortedDictionary<string, string>(StringComparer.Ordinal);
        var paramsArray = request["Params"]?.AsArray();
        if (paramsArray is not null)
        {
            foreach (var param in paramsArray)
            {
                var name = param?["Name"]?.GetValue<string>();
                var matchers = param?["Matchers"]?.AsArray();
                var value = matchers is not null && matchers.Count > 0
                    ? matchers[0]?["Pattern"]?.GetValue<string>() ?? ""
                    : "";

                if (name is not null)
                {
                    parameters[name] = value;
                }
            }
        }

        foreach (var kvp in parameters)
        {
            sb.Append(CultureInfo.InvariantCulture, $"{kvp.Key}={kvp.Value}&");
        }

        return sb.ToString();
    }

    private static string GenerateDeterministicGuid(string signature)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(signature));

        // Use first 16 bytes of hash to create a GUID
        var guidBytes = new byte[16];
        Array.Copy(hash, guidBytes, 16);

        // Set version (4) and variant bits for a valid UUID format
        guidBytes[6] = (byte)((guidBytes[6] & 0x0F) | 0x40); // Version 4
        guidBytes[8] = (byte)((guidBytes[8] & 0x3F) | 0x80); // Variant 1

        return new Guid(guidBytes).ToString();
    }

    private static string ReplaceGuidInFileName(string fileName, string oldGuid, string newGuid)
    {
        if (string.IsNullOrEmpty(oldGuid))
            return fileName;

        return fileName.Replace(oldGuid, newGuid, StringComparison.Ordinal);
    }

    private static void NormalizeHeaders(JsonNode node)
    {
        var headers = node["Response"]?["Headers"];
        if (headers is null) return;

        foreach (var header in _dynamicHeaders)
        {
            headers.AsObject().Remove(header);
        }

        foreach (var (header, pinned) in _pinnedHeaders)
        {
            if (headers[header] is not null)
            {
                headers[header] = pinned;
            }
        }
    }

    private static void ScrubVolatileBodyFields(JsonNode? node)
    {
        switch (node)
        {
            case JsonObject obj:
                // Snapshot the properties: the loop reassigns some of them.
                foreach (var property in obj.ToList())
                {
                    // Only pin actual values - a null stays null, so "no value recorded"
                    // does not turn into "recorded as zero".
                    if (_volatileBodyFields.TryGetValue(property.Key, out var pinned) && property.Value is JsonValue)
                    {
                        obj[property.Key] = pinned.DeepClone();
                    }
                    else
                    {
                        ScrubVolatileBodyFields(property.Value);
                    }
                }

                break;

            case JsonArray array:
                foreach (var item in array)
                {
                    ScrubVolatileBodyFields(item);
                }

                break;
        }
    }

    private static void SortUnorderedResponseBody(JsonNode node)
    {
        var path = node["Request"]?["Path"]?["Matchers"]?.AsArray() is { Count: > 0 } matchers
            ? matchers[0]?["Pattern"]?.GetValue<string>()
            : null;

        if (path is null || !_unorderedResponsePaths.Contains(path))
        {
            return;
        }

        SortRecursively(node["Response"]?["BodyAsJson"]);
    }

    private static void SortRecursively(JsonNode? node)
    {
        switch (node)
        {
            case JsonObject obj:
                // Key order varies too - these bodies are keyed by country - and JsonObject
                // preserves insertion order, so the properties need reordering as well.
                var properties = obj.Select(property => (property.Key, Value: property.Value?.DeepClone())).ToList();

                foreach (var property in properties)
                {
                    SortRecursively(property.Value);
                }

                obj.Clear();
                foreach (var (key, value) in properties.OrderBy(property => property.Key, StringComparer.Ordinal))
                {
                    obj[key] = value;
                }

                break;

            case JsonArray array:
                // Clone before clearing: a JsonNode keeps its parent, so the originals cannot be
                // re-added to the array they were just removed from.
                var items = array.Select(item => item?.DeepClone()).ToList();

                // Canonicalise children first, so the sort keys below are themselves stable.
                foreach (var item in items)
                {
                    SortRecursively(item);
                }

                array.Clear();
                foreach (var item in items.OrderBy(item => item?.ToJsonString() ?? string.Empty, StringComparer.Ordinal))
                {
                    array.Add(item);
                }

                break;
        }
    }

    /// <summary>
    /// Gets the default mappings directory for the test project.
    /// </summary>
    public static string GetDefaultMappingsDirectory()
    {
        for (var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory); dir is not null; dir = dir.Parent)
        {
            if (File.Exists(Path.Combine(dir.FullName, "TMDbLibTests.csproj")))
            {
                return Path.Combine(dir.FullName, "__wiremock__", "__admin", "mappings");
            }

            var sub = Path.Combine(dir.FullName, "TMDbLibTests");
            if (File.Exists(Path.Combine(sub, "TMDbLibTests.csproj")))
            {
                return Path.Combine(sub, "__wiremock__", "__admin", "mappings");
            }
        }

        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "__wiremock__", "__admin", "mappings");
    }
}

/// <summary>
/// Results from a normalization operation.
/// </summary>
public class NormalizationResult
{
    /// <summary>
    /// Files that were renamed to use deterministic GUIDs.
    /// </summary>
    public List<(string OldPath, string NewPath)> Renamed { get; } = [];

    /// <summary>
    /// Files that were updated in place (GUID already matched).
    /// </summary>
    public List<string> Updated { get; } = [];

    /// <summary>
    /// Duplicate files that were removed.
    /// </summary>
    public List<(string Deleted, string KeptDuplicate)> Duplicates { get; } = [];

    /// <summary>
    /// Files that were skipped (couldn't extract request signature).
    /// </summary>
    public List<string> Skipped { get; } = [];

    /// <summary>
    /// Files that encountered errors during processing.
    /// </summary>
    public List<(string File, string Error)> Errors { get; } = [];

    /// <summary>
    /// Returns a summary of the normalization results.
    /// </summary>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(CultureInfo.InvariantCulture, $"Renamed: {Renamed.Count}").AppendLine();
        sb.Append(CultureInfo.InvariantCulture, $"Updated: {Updated.Count}").AppendLine();
        sb.Append(CultureInfo.InvariantCulture, $"Duplicates removed: {Duplicates.Count}").AppendLine();
        sb.Append(CultureInfo.InvariantCulture, $"Skipped: {Skipped.Count}").AppendLine();
        sb.Append(CultureInfo.InvariantCulture, $"Errors: {Errors.Count}").AppendLine();

        if (Errors.Count > 0)
        {
            sb.AppendLine().AppendLine("Errors:");
            foreach (var (file, error) in Errors)
            {
                sb.Append(CultureInfo.InvariantCulture, $"  {Path.GetFileName(file)}: {error}").AppendLine();
            }
        }

        return sb.ToString();
    }
}
