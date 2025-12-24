using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
                if (node == null) continue;

                var signature = ExtractRequestSignature(node);
                if (signature == null)
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

                // Remove dynamic headers that change between recordings
                RemoveDynamicHeaders(node);

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
        if (request == null) return null;

        var sb = new StringBuilder();

        // Method
        var methods = request["Methods"]?.AsArray();
        if (methods != null && methods.Count > 0)
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
        if (pathMatchers != null && pathMatchers.Count > 0)
        {
            sb.Append(pathMatchers[0]?["Pattern"]?.GetValue<string>() ?? "");
        }
        sb.Append('|');

        // Query parameters (sorted for consistency)
        var parameters = new SortedDictionary<string, string>(StringComparer.Ordinal);
        var paramsArray = request["Params"]?.AsArray();
        if (paramsArray != null)
        {
            foreach (var param in paramsArray)
            {
                var name = param?["Name"]?.GetValue<string>();
                var matchers = param?["Matchers"]?.AsArray();
                var value = matchers != null && matchers.Count > 0
                    ? matchers[0]?["Pattern"]?.GetValue<string>() ?? ""
                    : "";

                if (name != null)
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

    private static void RemoveDynamicHeaders(JsonNode node)
    {
        var headers = node["Response"]?["Headers"];
        if (headers == null) return;

        // Headers that change between recordings and shouldn't affect matching
        var dynamicHeaders = new[]
        {
            "Date",
            "x-memc-age",
            "x-memc-expires",
            "x-task-id",
            "X-Amz-Cf-Id",
            "Age",
            "Via"
        };

        foreach (var header in dynamicHeaders)
        {
            if (headers[header] != null)
            {
                headers.AsObject().Remove(header);
            }
        }
    }

    /// <summary>
    /// Gets the default mappings directory for the test project.
    /// </summary>
    public static string GetDefaultMappingsDirectory()
    {
        for (var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory); dir != null; dir = dir.Parent)
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
