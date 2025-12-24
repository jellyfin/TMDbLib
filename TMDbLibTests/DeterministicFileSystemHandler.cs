using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using WireMock.Handlers;

namespace TMDbLibTests;

/// <summary>
/// A custom WireMock file system handler that generates deterministic filenames
/// based on request signatures (method + path + sorted query parameters).
/// This ensures that re-recording produces the same filenames for identical requests.
/// </summary>
public partial class DeterministicFileSystemHandler : IFileSystemHandler
{
    private readonly LocalFileSystemHandler _inner;
    private readonly HashSet<string> _writtenSignatures = new(StringComparer.Ordinal);

    /// <summary>
    /// Parameters to exclude from the signature calculation.
    /// These are dynamic values that change between requests but shouldn't affect mapping identity.
    /// </summary>
    private static readonly HashSet<string> _excludedParams = new(StringComparer.OrdinalIgnoreCase)
    {
        "request_token",   // Authentication tokens are dynamically generated
        "session_id",      // Session IDs change per session
        "guest_session_id"
    };

    /// <summary>
    /// Initializes a new instance with the specified root folder.
    /// </summary>
    /// <param name="rootFolder">The root folder for WireMock files.</param>
    public DeterministicFileSystemHandler(string rootFolder)
    {
        _inner = new LocalFileSystemHandler(rootFolder);
    }

    /// <inheritdoc />
    public string GetMappingFolder() => _inner.GetMappingFolder();

    /// <inheritdoc />
    public bool FolderExists(string path) => _inner.FolderExists(path);

    /// <inheritdoc />
    public void CreateFolder(string path) => _inner.CreateFolder(path);

    /// <inheritdoc />
    public IEnumerable<string> EnumerateFiles(string path, bool includeSubdirectories) =>
        _inner.EnumerateFiles(path, includeSubdirectories);

    /// <inheritdoc />
    public string ReadMappingFile(string path) => _inner.ReadMappingFile(path);

    /// <inheritdoc />
    public void WriteMappingFile(string path, string text)
    {
        // Parse the JSON to extract request signature
        JsonNode? node;
        try
        {
            node = JsonNode.Parse(text);
        }
        catch
        {
            // If parsing fails, write as-is
            _inner.WriteMappingFile(path, text);
            return;
        }

        if (node == null)
        {
            _inner.WriteMappingFile(path, text);
            return;
        }

        var signature = ExtractRequestSignature(node);
        if (signature == null)
        {
            _inner.WriteMappingFile(path, text);
            return;
        }

        // Check for duplicates - skip if we've already written this signature
        if (!_writtenSignatures.Add(signature))
        {
            // Duplicate request, don't write
            return;
        }

        // Generate deterministic GUID
        var deterministicGuid = GenerateDeterministicGuid(signature);

        // Update the JSON with deterministic GUID (UpdatedAt is preserved as-is)
        var oldGuid = node["Guid"]?.GetValue<string>() ?? string.Empty;
        node["Guid"] = deterministicGuid;

        // Remove dynamic headers
        RemoveDynamicHeaders(node);

        // Remove excluded params from request matchers (so WireMock can match during playback)
        RemoveExcludedParamsFromRequest(node);

        var updatedJson = node.ToJsonString(new JsonSerializerOptions { WriteIndented = true });

        // Calculate new filename
        var fileName = Path.GetFileName(path);
        var directory = Path.GetDirectoryName(path) ?? string.Empty;
        var newFileName = ReplaceGuidInFileName(fileName, oldGuid, deterministicGuid);
        var newPath = Path.Combine(directory, newFileName);

        // Delete old file with same GUID if it exists (from previous recording)
        DeleteExistingMappingWithGuid(directory, deterministicGuid);

        _inner.WriteMappingFile(newPath, updatedJson);
    }

    private void DeleteExistingMappingWithGuid(string directory, string newGuid)
    {
        if (!_inner.FolderExists(directory))
        {
            return;
        }

        // Find and delete any existing file with the same deterministic GUID
        // (from a previous recording session)
        foreach (var file in _inner.EnumerateFiles(directory, false))
        {
            if (file.Contains(newGuid, StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    _inner.DeleteFile(file);
                }
                catch
                {
                    // Ignore deletion errors
                }
            }
        }
    }

    /// <inheritdoc />
    public byte[] ReadResponseBodyAsFile(string path) => _inner.ReadResponseBodyAsFile(path);

    /// <inheritdoc />
    public string ReadResponseBodyAsString(string path) => _inner.ReadResponseBodyAsString(path);

    /// <inheritdoc />
    public void DeleteFile(string filename) => _inner.DeleteFile(filename);

    /// <inheritdoc />
    public bool FileExists(string filename) => _inner.FileExists(filename);

    /// <inheritdoc />
    public void WriteFile(string filename, byte[] bytes) => _inner.WriteFile(filename, bytes);

    /// <inheritdoc />
    public void WriteFile(string folder, string filename, byte[] bytes) => _inner.WriteFile(folder, filename, bytes);

    /// <inheritdoc />
    public byte[] ReadFile(string filename) => _inner.ReadFile(filename);

    /// <inheritdoc />
    public string ReadFileAsString(string filename) => _inner.ReadFileAsString(filename);

    /// <inheritdoc />
    public string GetUnmatchedRequestsFolder() => _inner.GetUnmatchedRequestsFolder();

    /// <inheritdoc />
    public void WriteUnmatchedRequest(string filename, string text) => _inner.WriteUnmatchedRequest(filename, text);

    private static string? ExtractRequestSignature(JsonNode node)
    {
        var request = node["Request"];
        if (request == null)
        {
            return null;
        }

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

                if (name != null && !_excludedParams.Contains(name))
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
        {
            return fileName;
        }

        return fileName.Replace(oldGuid, newGuid, StringComparison.Ordinal);
    }

    private static void RemoveDynamicHeaders(JsonNode node)
    {
        var headers = node["Response"]?["Headers"];
        if (headers == null)
        {
            return;
        }

        // Headers that change between recordings and shouldn't affect matching
        string[] dynamicHeaders =
        [
            "Date",
            "x-memc-age",
            "x-memc-expires",
            "x-task-id",
            "X-Amz-Cf-Id",
            "Age",
            "Via"
        ];

        foreach (var header in dynamicHeaders)
        {
            if (headers[header] != null)
            {
                headers.AsObject().Remove(header);
            }
        }
    }

    private static void RemoveExcludedParamsFromRequest(JsonNode node)
    {
        var paramsArray = node["Request"]?["Params"]?.AsArray();
        if (paramsArray == null)
        {
            return;
        }

        // Find indices of params to remove (iterate backwards to avoid index issues)
        for (var i = paramsArray.Count - 1; i >= 0; i--)
        {
            var paramName = paramsArray[i]?["Name"]?.GetValue<string>();
            if (paramName != null && _excludedParams.Contains(paramName))
            {
                paramsArray.RemoveAt(i);
            }
        }
    }
}
