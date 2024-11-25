using System.Net;
using Com.FirstSolver.Splash;
using EvoComms.Devices.HanvonVF.Messages.Handlers;
using EvoComms.Devices.HanvonVF.SM4Encryption;
using Microsoft.Extensions.Logging;
using Mina.Filter.Codec;
using Mina.Transport.Socket;

namespace EvoComms.Devices.HanvonVF.HanvonServer;

public class HanvonListener : IAsyncDisposable
{
    private readonly HanvonHandlerRegistry _handlerRegistry;
    private readonly ILogger<HanvonListener> _logger;

    private readonly SM2UserInformation _sm2UserInformation =
        SM2KeyFactory.CreateServerInfo("Com.ClockingSystems.EvoComms");

    private readonly AsyncSocketAcceptor _tcpServer;

    public HanvonListener(
        int port,
        AsyncSocketAcceptor server,
        ILogger<HanvonListener> logger,
        HanvonHandlerRegistry handlerRegistry)
    {
        Port = port;
        _tcpServer = server;
        _logger = logger;
        _handlerRegistry = handlerRegistry;
        _tcpServer.FilterChain.AddLast("codec", new ProtocolCodecFilter(new FaceReaderProtocolCodecFactory()));
        MapEventHandlers();
    }

    private int Port { get; }
    public bool IsListening => _tcpServer.Active;

    public async ValueTask DisposeAsync()
    {
        await StopAsync();
    }

    public void Start()
    {
        if (!_tcpServer.Active)
        {
            var endpoint = new IPEndPoint(IPAddress.Any, Port);
            _tcpServer.Bind(endpoint);
        }
    }

    public async Task StopAsync()
    {
        if (!_tcpServer.Active) return;
        _tcpServer.Unbind();
        _tcpServer.Dispose();
        await Task.CompletedTask;
    }

    private void MapEventHandlers()
    {
        _tcpServer.SessionOpened += (_, ea) =>
        {
            FaceReaderProtocolCodecFactory.EnablePassiveEncryption(ea.Session, true, _sm2UserInformation);
            _logger.LogInformation(
                $"Hanvon: New connection from {ea.Session.RemoteEndPoint} on ID: {ea.Session.Id}. Local Endpoint: {ea.Session.LocalEndPoint}");
        };

        _tcpServer.MessageReceived +=
            async (_, ea) =>
            {
                var messageString = (string)ea.Message;
                _logger.LogInformation(
                    $"Message Received from {ea.Session.RemoteEndPoint} with Session ID {ea.Session.Id}: {messageString}");
                var handler = _handlerRegistry.GetHandler(messageString);
                if (handler == null)
                {
                    _logger.LogWarning(
                        $"No handler found for message: {messageString}. Received from {ea.Session.RemoteEndPoint} on session: {ea.Session.Id}");
                    return;
                }

                await handler.HandleMessageAsync(
                    ea.Session,
                    ea.Message);
            };
    }
}