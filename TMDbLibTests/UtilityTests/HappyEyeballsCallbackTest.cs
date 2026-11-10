using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Rest;
using Xunit;

namespace TMDbLibTests.UtilityTests;

/// <summary>
/// Exercises <see cref="HappyEyeballsCallback"/> end-to-end through a <see cref="SocketsHttpHandler"/>
/// against a local listener.
/// </summary>
public class HappyEyeballsCallbackTest
{
    /// <summary>
    /// "localhost" usually resolves to both ::1 and 127.0.0.1. With the listener bound to IPv4 only,
    /// the IPv6 attempt is refused and the callback must fall back to IPv4.
    /// </summary>
    [Fact]
    public async Task ConnectAsync_FallsBackToIPv4WhenIPv6IsRefused()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;

        var serverTask = ServeSingleResponseAsync(listener, TestContext.Current.CancellationToken);

        using var handler = new SocketsHttpHandler { ConnectCallback = HappyEyeballsCallback.ConnectAsync, UseProxy = false };
        using var client = new HttpClient(handler);

        var body = await client.GetStringAsync(new System.Uri($"http://localhost:{port}/"), TestContext.Current.CancellationToken);

        Assert.Equal("ok", body);
        await serverTask;
    }

    /// <summary>
    /// A losing connection attempt must not leave a faulted task behind that later surfaces as
    /// <see cref="TaskScheduler.UnobservedTaskException"/> (https://github.com/jellyfin/TMDbLib/issues/674).
    /// </summary>
    [Fact]
    public async Task ConnectAsync_DoesNotLeaveUnobservedExceptions()
    {
        var unobserved = 0;
        void OnUnobserved(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            if (e.Exception.ToString().Contains(nameof(HappyEyeballsCallback), System.StringComparison.Ordinal))
            {
                Interlocked.Increment(ref unobserved);
            }
        }

        TaskScheduler.UnobservedTaskException += OnUnobserved;
        try
        {
            await ConnectAsync_FallsBackToIPv4WhenIPv6IsRefused();

            for (var i = 0; i < 3; i++)
            {
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
            }

            Assert.Equal(0, Volatile.Read(ref unobserved));
        }
        finally
        {
            TaskScheduler.UnobservedTaskException -= OnUnobserved;
        }
    }

    /// <summary>
    /// When nothing is listening, the failure surfaces as an <see cref="HttpRequestException"/>
    /// rather than hanging or crashing.
    /// </summary>
    [Fact]
    public async Task ConnectAsync_ThrowsWhenAllAttemptsFail()
    {
        int port;
        using (var probe = new TcpListener(IPAddress.Loopback, 0))
        {
            probe.Start();
            port = ((IPEndPoint)probe.LocalEndpoint).Port;
        }

        using var handler = new SocketsHttpHandler { ConnectCallback = HappyEyeballsCallback.ConnectAsync, UseProxy = false };
        using var client = new HttpClient(handler);

        await Assert.ThrowsAsync<HttpRequestException>(
            () => client.GetStringAsync(new System.Uri($"http://localhost:{port}/"), TestContext.Current.CancellationToken));
    }

    private static async Task ServeSingleResponseAsync(TcpListener listener, CancellationToken cancellationToken)
    {
        using var socket = await listener.AcceptSocketAsync(cancellationToken);
        var buffer = new byte[4096];
        await socket.ReceiveAsync(buffer, SocketFlags.None, cancellationToken);

        var response = Encoding.ASCII.GetBytes("HTTP/1.1 200 OK\r\nContent-Length: 2\r\nConnection: close\r\n\r\nok");
        await socket.SendAsync(response, SocketFlags.None, cancellationToken);
        socket.Shutdown(SocketShutdown.Both);
    }
}
