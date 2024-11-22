using System.Net;
using Com.FirstSolver.Splash;
using EvoComms.Core.Filesystem.Settings;
using EvoComms.Devices.HanvonVF.Settings;
using Microsoft.Extensions.Logging;
using Mina.Core.Session;
using Mina.Filter.Codec;
using Mina.Transport.Socket;

namespace EvoComms.Devices.HanvonVF.HanvonServer;

public abstract class BaseHanvonTcpServer
{
    protected readonly ILogger _logger;
    protected HanvonSettingsProvider _hanvonSettingsProvider;
    protected ModuleSettings? settings;

    protected SM2UserInformation SM2ServerInfo = new(
        "Com.FirstSolver.Splash",
        "DE078F1052116A4F706288AB9EC8E10009EAA510B68B68D917AA63F3D07B3876",
        "59C9ADD36106ABD5D26CCF7F92D8BBAD33A0C55D0CF9F644CF97D340EDDFB949",
        "D781D3BBD4F1B8263DBF68613CF7E830BBD6CC569740303D4DDEAEB5216AE639"
    );

    protected AsyncSocketAcceptor? TcpServer;

    public BaseHanvonTcpServer(ILogger logger, HanvonSettingsProvider hanvonSettingsProvider)
    {
        _logger = logger;
    }

    public abstract void Start();
    public abstract void Stop();

    protected void SendMessageToClient(string ipAddress, int port, string message)
    {
        AsyncSocketConnector connector = new();
        connector.FilterChain.AddLast("codec", new ProtocolCodecFilter(new FaceReaderProtocolCodecFactory()));

        connector.MessageReceived += (o, ea) =>
        {
            // Log the received message
            _logger.LogDebug($"Hanvon: Received message from {ipAddress}:{port}: {(string)ea.Message}");
        };

        var connectFuture = connector.Connect(new IPEndPoint(IPAddress.Parse(ipAddress), port));
        connectFuture.Await();

        if (connectFuture.Connected)
        {
            var session = connectFuture.Session;
            FaceReaderProtocolCodecFactory.EnablePassiveEncryption(session, true, SM2ServerInfo);
            session.Config.SetIdleTime(IdleStatus.BothIdle, 10);

            var writeFuture = session.Write(message);
            writeFuture.Await();

            // Wait for the device's response
            Thread.Sleep(2000);

            session.Close(true);
            connector.Dispose();
        }
        else
        {
            // Handle connection error
            _logger.LogInformation($"Hanvon: Failed to connect to {ipAddress}:{port}");
        }
    }
}