using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace TMDbLib.Rest;

internal class RestResponse : IDisposable
{
    private readonly HttpResponseMessage? _response;

    public RestResponse(HttpResponseMessage? response)
    {
        _response = response;
    }

    public bool IsValid => _response is not null;

    public HttpStatusCode StatusCode => _response?.StatusCode ?? HttpStatusCode.NotFound;

    public async Task<Stream> GetContent()
    {
        if (_response is null)
        {
            throw new InvalidOperationException("Cannot get content from a null response");
        }

        return await _response.Content.ReadAsStreamAsync().ConfigureAwait(false);
    }

    public string? GetHeader(string name, string? @default = null)
    {
        return _response?.Headers.GetValues(name).FirstOrDefault() ?? @default;
    }

    public virtual void Dispose()
    {
        _response?.Dispose();
    }
}
