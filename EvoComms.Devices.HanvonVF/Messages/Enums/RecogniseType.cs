using System.Text.Json.Serialization;

namespace EvoComms.Devices.HanvonVF.Messages.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RecogniseType
{
    FaceOnly = 0,
    CardAndFace = 1,
    CardOrFace = 2,
    CardOnly = 3,
    IdAndFace = 4,
    IdOrFace = 5,
    IdAndCard = 6,
    IdOrCard = 7,
    DeviceDefault = 8
}