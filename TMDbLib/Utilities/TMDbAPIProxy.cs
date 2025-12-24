using System;
using System.Net;

namespace TMDbLib.Utilities;

/// <summary>
/// Represents a Web Proxy to use for TMDb API Requests.
/// </summary>
/// <remarks>
/// This is a very simple implementation of a Web Proxy to be used when requesting data from TMDb API.
/// It does not support proxy bypassing or multi-proxy configuration based on the destination URL, for instance.
/// </remarks>
public class TMDbAPIProxy : IWebProxy
{
    private readonly Uri _proxyUri;

    /// <summary>
    /// Initializes a new instance of the <see cref="TMDbAPIProxy"/> class.
    /// </summary>
    /// <param name="proxyUri">The URI of the proxy server.</param>
    /// <param name="credentials">The credentials to use for authenticating with the proxy server. Optional.</param>
    public TMDbAPIProxy(Uri proxyUri, ICredentials? credentials = null)
    {
        ArgumentNullException.ThrowIfNull(proxyUri);

        _proxyUri = proxyUri;
        Credentials = credentials;
    }

    /// <summary>
    /// Gets or sets the credentials to use for authenticating in the proxy server.
    /// </summary>
    public ICredentials? Credentials { get; set; }

    /// <summary>
    /// Gets the proxy server <see cref="Uri"/> to be used when accessing <paramref name="destination"/>.
    /// </summary>
    /// <param name="destination">The destination URL to be accessed.</param>
    /// <returns>Proxy URI.</returns>
    public Uri GetProxy(Uri destination)
    {
        return _proxyUri;
    }

    /// <summary>
    /// Determines whether the proxy should be bypassed for the specified host.
    /// </summary>
    /// <param name="host">The host URI to check.</param>
    /// <returns>Always returns false as this proxy does not support bypassing.</returns>
    public bool IsBypassed(Uri host)
    {
        return false;
    }
}
