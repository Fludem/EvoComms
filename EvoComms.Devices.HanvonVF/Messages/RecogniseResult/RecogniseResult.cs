using System.Text.Json;
using System.Text.Json.Serialization;
using EvoComms.Devices.HanvonVF.Messages.Enums;

namespace EvoComms.Devices.HanvonVF.Messages.RecogniseResult;

public class RecogniseResult
{
    public RecogniseResultCommand Command { get; set; }
    public RecogniseResultResponse Response { get; set; }
}

public class RecogniseResultCommand : IHanvonMessage<RecogniseResultCommandParam>
{
    [JsonPropertyName("COMMAND")] public string COMMAND { get; set; } = "RecogniseResult";

    [JsonPropertyName("PARAM")] public required RecogniseResultCommandParam PARAM { get; set; }
}

public class RecogniseResultCommandParam
{
    /// <summary>
    ///     SN Number
    /// </summary>
    [JsonPropertyName("sn")]
    public required string SerialNumber { get; set; }

    /// <summary>
    ///     User ID
    /// </summary>
    [JsonPropertyName("id")]
    public required string UserId { get; set; }

    /// <summary>
    ///     Name
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    ///     Device Model
    /// </summary>
    [JsonPropertyName("type")]
    public string? DeviceModel { get; set; }

    /// <summary>
    ///     Algorithm Version
    /// </summary>
    [JsonPropertyName("alg_edition")]
    public string? AlgorithmVersion { get; set; }

    [JsonPropertyName("isUserUpdate")] public bool? IsUserUpdate { get; set; }

    /// <summary>
    ///     Matching Picture (Base64 Code)
    /// </summary>
    [JsonPropertyName("capturejpg")]
    public string? UserPicture { get; set; }

    /// <summary>
    ///     Matching Score
    /// </summary>
    [JsonPropertyName("score")]
    public float? Score { get; set; }

    /// <summary>
    ///     1/0 (Pass/Fail)
    /// </summary>
    [JsonPropertyName("pass")]
    public int? Pass { get; set; }

    /// <summary>
    ///     Threshold
    /// </summary>
    [JsonPropertyName("threshold")]
    public float? Threshold { get; set; }

    /// <summary>
    ///     Clocking Time (yyyy-mm-dd hh:mm:ss)
    /// </summary>
    [JsonPropertyName("time")]
    public DateTime Time { get; set; }

    /// <summary>
    ///     Recognition method
    /// </summary>
    [JsonPropertyName("recogType")]
    [JsonConverter(typeof(RecogniseTypeConverter))]
    public RecogniseType? RecogniseType { get; set; }

    /// <summary>
    ///     No meaning for VF1000
    /// </summary>
    [JsonPropertyName("identifyType")]
    public string? RecogniseMethod { get; set; }

    /// <summary>
    ///     Temperature
    /// </summary>
    [JsonPropertyName("animalHeat")]
    public float? Temperature { get; set; }

    /// <summary>
    ///     1 = Wear mask, 2 = Not Wear Mask
    /// </summary>
    [JsonPropertyName("wearMask")]
    public int? WearMask { get; set; }
}

public class RecogniseTypeConverter : JsonConverter<RecogniseType?>
{
    public override RecogniseType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            var value = reader.GetInt32();
            if (Enum.IsDefined(typeof(RecogniseType), value)) return (RecogniseType)value;
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, RecogniseType? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
            writer.WriteNumberValue((int)value.Value);
        else
            writer.WriteNullValue();
    }
}