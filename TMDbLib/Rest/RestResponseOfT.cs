using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace TMDbLib.Rest;

#pragma warning disable SA1649 // File name should match first type name (generic types cannot match file names)
internal class RestResponse<T> : RestResponse
#pragma warning restore SA1649
{
    private readonly RestClient _client;

    public RestResponse(HttpResponseMessage? response, RestClient client)
        : base(response)
    {
        _client = client;
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026:RequiresUnreferencedCode", Justification = "T is always a TMDbLib response type referenced by the calling public client method; the linker preserves it.")]
    [UnconditionalSuppressMessage("AOT", "IL3050:RequiresDynamicCode", Justification = "T is always a TMDbLib response type referenced by the calling public client method; no runtime type construction is performed here.")]
    public async Task<T?> GetDataObject()
    {
        if (!IsValid)
        {
            return default;
        }

        using Stream content = await GetContent().ConfigureAwait(false);
        var result = _client.Serializer.Deserialize(content, typeof(T));

        return result is T typed ? typed : default;
    }
}
