using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using EvoComms.Core.Models;
using EvoComms.Devices.Timy.Messages.Shared;
using Newtonsoft.Json;

namespace EvoComms.Devices.Timy.Messages.ServerToTerminal;

public class GetAllLogCommand : BaseServerRequest
{
    [SetsRequiredMembers]
    public GetAllLogCommand(DateTime startDate, DateTime endDate)
    {
        Command = "getalllog";
        EndDate = endDate.ToString("yyyy-MM-dd");
        StartDate = startDate.ToString("yyyy-MM-dd");
    }

    [JsonPropertyName("stn")] public bool FirstPacket { get; set; } = true;
    [JsonPropertyName("from")] public string StartDate { get; set; }
    [JsonPropertyName("to")] public string EndDate { get; set; }
}

public class GetAllLogResponse : BaseDeviceResponse
{
    [JsonPropertyName("count")] public int TotalClockingsCount { get; set; }
    [JsonPropertyName("from")] public int StartIndex { get; set; }
    [JsonPropertyName("to")] public int EndIndex { get; set; }

    public required List<Record> Records { get; set; }

    public List<Record> GetRecords()
    {
        foreach (var clocking in
                 Records) clocking.DeviceSerial = "NA";

        return Records;
    }
}

public class ContinueAllLog : BaseServerRequest
{
    [SetsRequiredMembers]
    public ContinueAllLog()
    {
        Command = "getalllog";
    }

    [JsonPropertyName("stn")] public bool FirstPacket { get; set; } = false;
}