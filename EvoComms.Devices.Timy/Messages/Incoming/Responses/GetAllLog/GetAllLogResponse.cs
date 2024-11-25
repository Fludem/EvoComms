using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using EvoComms.Core.Models;
using EvoComms.Devices.Timy.Messages.Outgoing.Commands.GetAllLog;
using Newtonsoft.Json;
using NLog;

namespace EvoComms.Devices.Timy.Messages.Incoming.Responses.GetAllLog;

public class GetAllLogResponse : BaseIncomingResponse
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    [JsonPropertyName("count")] public int TotalRecordsCount { get; set; }

    [JsonPropertyName("from")] public int FromDate { get; set; }

    [JsonPropertyName("to")] public int EndDate { get; set; }


    public required List<Record> Records { get; set; }

    public string Response()
    {
        var response = JsonConvert.SerializeObject(new ContinueAllLog());
        return response;
    }

    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        foreach (var clocking in Records) clocking.DeviceSerialNumber = SerialNumber;
    }

    public List<Record> GetRecords()
    {
        foreach (var clocking in
                 Records) clocking.DeviceSerialNumber = SerialNumber; // Assign SN from GetAllLogResponse to each Record

        return Records;
    }
}