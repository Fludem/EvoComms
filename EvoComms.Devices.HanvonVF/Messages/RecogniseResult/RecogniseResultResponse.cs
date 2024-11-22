using System.Text.Json.Serialization;

namespace EvoComms.Devices.HanvonVF.Messages.RecogniseResult;

public class RecogniseResultResponse : IHanvonResponseMessage<RecogniseResultParam>
{
    [JsonPropertyName("RETURN")] public string RETURN { get; set; } = "RecogniseResult";

    [JsonPropertyName("PARAM")] public RecogniseResultParam PARAM { get; set; }
}

public class RecogniseResultParam
{
    [JsonPropertyName("reason")] public string? Reason { get; set; }

    [JsonPropertyName("result")] public string Result { get; set; }
}