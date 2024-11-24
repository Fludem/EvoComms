// using System.Net;
// using Com.FirstSolver.Splash;
// using EvoComms.Devices.HanvonVF.Messages;
// using EvoComms.Devices.HanvonVF.Settings;
// using Microsoft.Extensions.Logging;
// using Mina.Filter.Codec;
// using Mina.Transport.Socket;
// using Newtonsoft.Json;
//
// namespace EvoComms.Devices.HanvonVF.HanvonServer;
//
// public class ServiceHanvonListener : BaseHanvonListener
// {
//     public ServiceHanvonListener(ILogger<ServiceHanvonListener> logger,
//         HanvonSettingsProvider hanvonSettingsProvider) : base(
//         logger, hanvonSettingsProvider)
//     {
//         Logger.LogInformation("Hanvon Listener Instantiated.");
//     }
//
//     public override void Start()
//     {
//         Logger.LogInformation("Hanvon Listener Starting.");
//         TcpServer = new AsyncSocketAcceptor();
//
//         TcpServer.FilterChain.AddLast("codec", new ProtocolCodecFilter(new FaceReaderProtocolCodecFactory()));
//         TcpServer.SessionOpened += (o, ea) =>
//         {
//             FaceReaderProtocolCodecFactory.EnablePassiveEncryption(ea.Session, true, SM2ServerInfo);
//         };
//         TcpServer.MessageReceived += (o, ea) =>
//         {
//             Logger.LogInformation($"Message Received: {(string)ea.Message}");
//             var hanvonMessage = JsonConvert.DeserializeObject<HanvonMessage>((string)ea.Message);
//
//             if (hanvonMessage.COMMAND == "RecogniseResult")
//             {
//                 REPLY_PCIR_TYPE M = new();
//                 M.RETURN = "RecogniseResult";
//                 M.PARAM = new PARAM_REPLY_PCIR_TYPE();
//                 M.PARAM.result = "success";
//                 ea.Session.Write(M.ToJsonString());
//                 // BioTotalClockingWriter clockingWriter = new();
//                 // var dateTime = DateTime.ParseExact(hanvonMessage.PARAM.time, "yyyy-MM-dd HH:mm:ss",
//                 //     CultureInfo.InvariantCulture);
//                 // var reformattedTime = dateTime.ToString("dd/MM/yyyy  HH:mm:ss");
//                 // clockingWriter.WriteClockingFile(hanvonMessage.PARAM.id, reformattedTime, settings.OutputPath);
//                 // _logger.LogInformation(
//                 //     $"Hanvon: Clocking file written for user {hanvonMessage.PARAM.id} at {reformattedTime} to path {settings.OutputPath}");
//             }
//         };
//
//         IPEndPoint endpoint = new(IPAddress.Any, 9910);
//         TcpServer.Bind(endpoint);
//         Logger.LogInformation("Hanvon TCP Server started on port: " + 9910);
//     }
//
//     public override void Stop()
//     {
//         if (TcpServer != null)
//         {
//             TcpServer.Unbind();
//             TcpServer.Dispose();
//             TcpServer = null;
//             Logger.LogInformation("Hanvon Server Module Stopped");
//         }
//     }
// }

