using System.Text.Json.Serialization;
using EvoComms.Core.Models;

namespace EvoComms.Devices.Timy.Messages.Incoming.Commands.SendLogs;

public class SendLogCommand : BaseCommand
{
    [JsonPropertyName("sn")] public required string SerialNumber { get; set; } = "1";
    [JsonPropertyName("count")] public int Count { get; set; }
    [JsonPropertyName("logindex")] public int PaginationIndex { get; set; }
    [JsonPropertyName("record")] public required List<Record> Records { get; set; }

    public string Response()
    {
        return "{\"ret\":\"sendlog\",\"count\":" + Count + ",\"logindex\":" + PaginationIndex +
               ",\"result\":true,\"cloudtime\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
               "\"}";
    }

    public List<Record> GetRecords()
    {
        foreach (var record in Records) record.DeviceSerialNumber = SerialNumber;

        return Records;
    }
}