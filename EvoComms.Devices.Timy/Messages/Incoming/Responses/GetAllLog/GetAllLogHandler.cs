using EvoComms.Core.Services;
using EvoComms.Devices.Timy.Messages.Attributes;
using EvoComms.Devices.Timy.Settings;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SuperSocket.WebSocket.Server;

namespace EvoComms.Devices.Timy.Messages.Incoming.Responses.GetAllLog;

[TimyResponseHandler("getalllog")]
public class GetAllLogHandler : BaseMessageHandler
{
    public GetAllLogHandler(ILogger<GetAllLogHandler> logger,
        RecordService recordService,
        TimySettingsProvider timySettingsProvider) : base(logger, recordService, timySettingsProvider)
    {
    }

    public override async Task Handle(WebSocketSession session, string message)
    {
        try
        {
            var allLogResponse = JsonConvert.DeserializeObject<GetAllLogResponse>(message) ??
                                 throw new InvalidOperationException();
            _logger.LogDebug(
                $"Serialized Get All Log Response. Serial - {allLogResponse.SerialNumber} | Total Count - {allLogResponse.TotalRecordsCount} | Records Index Start: {allLogResponse.FromDate} | Records Index End: {allLogResponse.EndDate}");

            if (allLogResponse.TotalRecordsCount > allLogResponse.EndDate)
            {
                _logger.LogInformation(
                    $"Device {allLogResponse.SerialNumber} has {allLogResponse.TotalRecordsCount - allLogResponse.EndDate} clockings left to collect. Clockings received so far: {allLogResponse.EndDate} out of {allLogResponse.TotalRecordsCount}");
                await session.SendAsync(allLogResponse.Response());
            }

            else
            {
                _logger.LogInformation(
                    $"Timy: Device {allLogResponse.SerialNumber} has finished sending all clockings. Total Clockings: {allLogResponse.TotalRecordsCount}");
            }

            var settings = await _settingsProvider.LoadSettings();
            await RecordService.ProcessClockings(allLogResponse.GetRecords(), allLogResponse.SerialNumber, settings);
        }
        catch (Exception e)
        {
            _logger.LogError(
                $"Error Deserializing Message For Device On IP {session.RemoteEndPoint}. Error: {e.Message} - {e.InnerException}");
        }
    }
}