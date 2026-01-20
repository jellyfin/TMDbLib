using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace TMDbLib.Rest;

/// <summary>
/// Implements Happy Eyeballs (RFC 8305) connection algorithm for HttpClient.
/// This attempts IPv6 connections first, then falls back to IPv4 if IPv6 fails or is slow.
/// Based on https://slugcat.systems/post/24-06-16-ipv6-is-hard-happy-eyeballs-dotnet-httpclient/.
/// </summary>
internal static class HappyEyeballsCallback
{
    /// <summary>
    /// Delay between connection attempts as recommended by RFC 8305.
    /// </summary>
    private const int ConnectionAttemptDelayMs = 250;

    /// <summary>
    /// Connect callback that implements Happy Eyeballs algorithm.
    /// </summary>
    /// <param name="context">The connection context containing DNS endpoint information.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A network stream for the established connection.</returns>
    public static async ValueTask<Stream> ConnectAsync(
        SocketsHttpConnectionContext context,
        CancellationToken cancellationToken)
    {
        var endPoint = context.DnsEndPoint;

        // Resolve DNS to get all IP addresses
        var resolvedAddresses = await GetAddressesAsync(endPoint.Host, cancellationToken).ConfigureAwait(false);

        if (resolvedAddresses.Length == 0)
        {
            throw new SocketException((int)SocketError.HostNotFound);
        }

        // Interleave IPv6 and IPv4 addresses (IPv6 first per RFC 8305)
        var sortedAddresses = SortInterleaved(resolvedAddresses);

        // Attempt connections with Happy Eyeballs algorithm
        var socket = await ConnectWithHappyEyeballsAsync(
            sortedAddresses,
            endPoint.Port,
            TimeSpan.FromMilliseconds(ConnectionAttemptDelayMs),
            cancellationToken).ConfigureAwait(false);

        return new NetworkStream(socket, ownsSocket: true);
    }

    private static async Task<IPAddress[]> GetAddressesAsync(string host, CancellationToken cancellationToken)
    {
        // If host is already an IP address, return it directly
        if (IPAddress.TryParse(host, out var ip))
        {
            return [ip];
        }

        var entry = await Dns.GetHostEntryAsync(host, cancellationToken).ConfigureAwait(false);
        return entry.AddressList;
    }

    /// <summary>
    /// Sorts addresses by interleaving IPv6 and IPv4, with IPv6 first.
    /// This ensures IPv6 is attempted first but IPv4 follows quickly.
    /// </summary>
    private static IPAddress[] SortInterleaved(IPAddress[] addresses)
    {
        var ipv6 = addresses.Where(x => x.AddressFamily == AddressFamily.InterNetworkV6).ToArray();
        var ipv4 = addresses.Where(x => x.AddressFamily == AddressFamily.InterNetwork).ToArray();

        var commonLength = Math.Min(ipv6.Length, ipv4.Length);
        var result = new IPAddress[addresses.Length];

        // Interleave: IPv6, IPv4, IPv6, IPv4, ...
        for (var i = 0; i < commonLength; i++)
        {
            result[i * 2] = ipv6[i];
            result[(i * 2) + 1] = ipv4[i];
        }

        // Append remaining addresses
        if (ipv4.Length > ipv6.Length)
        {
            ipv4.AsSpan(commonLength).CopyTo(result.AsSpan(commonLength * 2));
        }
        else if (ipv6.Length > ipv4.Length)
        {
            ipv6.AsSpan(commonLength).CopyTo(result.AsSpan(commonLength * 2));
        }

        return result;
    }

    /// <summary>
    /// Attempts connections to multiple addresses with staggered starts per RFC 8305.
    /// </summary>
    private static async Task<Socket> ConnectWithHappyEyeballsAsync(
        IPAddress[] addresses,
        int port,
        TimeSpan delay,
        CancellationToken cancellationToken)
    {
        using var successCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        var allTasks = new List<Task<Socket>>();
        var pendingTasks = new List<Task<Socket>>();
        Socket? successSocket = null;
        var successIndex = -1;

        try
        {
            while (successSocket == null && (allTasks.Count < addresses.Length || pendingTasks.Count > 0))
            {
                // Start a new connection attempt if we haven't tried all addresses
                if (allTasks.Count < addresses.Length)
                {
                    var newTask = AttemptConnectionAsync(addresses[allTasks.Count], port, successCts.Token);
                    pendingTasks.Add(newTask);
                    allTasks.Add(newTask);
                }

                var whenAnyDone = Task.WhenAny(pendingTasks);

                if (allTasks.Count < addresses.Length)
                {
                    // Wait for either a connection to complete or the delay to expire
                    var delayTask = Task.Delay(delay, successCts.Token);
                    var completedFirst = await Task.WhenAny(whenAnyDone, delayTask).ConfigureAwait(false);

                    if (completedFirst == delayTask)
                    {
                        // Delay expired, start next connection attempt
                        continue;
                    }
                }

                // A connection attempt completed
                var completedTask = await whenAnyDone.ConfigureAwait(false);

                if (completedTask.IsCompletedSuccessfully)
                {
                    successSocket = await completedTask.ConfigureAwait(false);
                    successIndex = allTasks.IndexOf(completedTask);
                    break;
                }

                // Connection failed, remove from pending and try next
                pendingTasks.Remove(completedTask);
            }

            cancellationToken.ThrowIfCancellationRequested();

            if (successSocket == null)
            {
                // All connections failed - aggregate the exceptions
                var exceptions = allTasks
                    .Where(x => x.IsFaulted)
                    .SelectMany(x => x.Exception!.InnerExceptions)
                    .ToList();

                throw new AggregateException("All connection attempts failed.", exceptions);
            }

            return successSocket;
        }
        finally
        {
            // Cancel remaining attempts
            await successCts.CancelAsync().ConfigureAwait(false);

            // Dispose any successful sockets that weren't the winner
            for (var i = 0; i < allTasks.Count; i++)
            {
                var task = allTasks[i];
                if (task.IsCompletedSuccessfully && i != successIndex)
                {
                    (await task.ConfigureAwait(false)).Dispose();
                }
            }
        }
    }

    private static async Task<Socket> AttemptConnectionAsync(IPAddress address, int port, CancellationToken cancellationToken)
    {
        var socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
        {
            NoDelay = true
        };

        try
        {
            await socket.ConnectAsync(new IPEndPoint(address, port), cancellationToken).ConfigureAwait(false);
            return socket;
        }
        catch
        {
            socket.Dispose();
            throw;
        }
    }
}
